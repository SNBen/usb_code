#include "stdafx.h"
#include <websocketpp/config/asio_no_tls_client.hpp>
#include <websocketpp/client.hpp>

#include <websocketpp/common/thread.hpp>
#include <websocketpp/common/memory.hpp>

#include <cstdlib>
#include <iostream>
#include <map>
#include <string>
#include <sstream>
#include "connection_metadata.hpp"
#include "websocket_endpoint.h"

typedef websocketpp::client<websocketpp::config::asio_client> ws_client;

ws_client g_wsEndPoint;
connection_metadata::ptr g_wsClientConnection;
websocketpp::lib::shared_ptr<websocketpp::lib::thread> g_threadWS;

websocket_endpoint::websocket_endpoint()
{
    g_wsEndPoint.clear_access_channels(websocketpp::log::alevel::all);
    g_wsEndPoint.clear_error_channels(websocketpp::log::alevel::all);
    g_wsEndPoint.init_asio();
    g_wsEndPoint.start_perpetual();
    g_threadWS = websocketpp::lib::make_shared<websocketpp::lib::thread>(&ws_client::run, &g_wsEndPoint);
}

websocket_endpoint::~websocket_endpoint()
{
    g_wsEndPoint.start_perpetual();

    if (g_wsClientConnection->get_status() == "Open")
    {
        websocketpp::lib::error_code ec;
        g_wsEndPoint.close(g_wsClientConnection->get_hdl(), websocketpp::close::status::going_away, "", ec);

        if (ec)
        {
            std::cout << "> Error closing ws connection" 
                << g_wsClientConnection->get_uri()
                << " : " 
                << ec.message() << std::endl;
        }
    }

    g_threadWS->join();
}

int websocket_endpoint::connect(std::string const &uri)
{
    websocketpp::lib::error_code ec;
    ws_client::connection_ptr pConnection = g_wsEndPoint.get_connection(uri, ec);

    if (ec)
    {
        std::cout << "> Connection initializaion error: " << ec.message() << std::endl;
        return -1;
    }

    g_wsClientConnection = websocketpp::lib::make_shared<connection_metadata>(pConnection->get_handle(), uri);

    pConnection->set_open_handler(websocketpp::lib::bind(
        &connection_metadata::on_open,
        g_wsClientConnection,
        &g_wsEndPoint,
        websocketpp::lib::placeholders::_1
    ));

    pConnection->set_fail_handler(websocketpp::lib::bind(
        &connection_metadata::on_fail,
        g_wsClientConnection,
        &g_wsEndPoint,
        websocketpp::lib::placeholders::_1
    ));

    pConnection->set_close_handler(websocketpp::lib::bind(
        &connection_metadata::on_close,
        g_wsClientConnection,
        &g_wsEndPoint,
        websocketpp::lib::placeholders::_1
    ));

    pConnection->set_message_handler(websocketpp::lib::bind(
        &connection_metadata::on_message,
        g_wsClientConnection,
        websocketpp::lib::placeholders::_1,
        websocketpp::lib::placeholders::_2
    ));

    g_wsEndPoint.connect(pConnection);
    return 0;
}

void close(websocketpp::close::status::value code, std::string reason)
{
    websocketpp::lib::error_code ec;

    g_wsEndPoint.close(g_wsClientConnection->get_hdl(), code, reason, ec);
    if (ec)
    {
        std::cout << "> Error initiating close:" << ec.message() << std::endl;
    }
}

void websocket_endpoint::close()
{
    if (g_wsClientConnection->get_status() == "Open")
    {
        int close_code = websocketpp::close::status::normal;
        ::close(close_code, "");
    }
}

void websocket_endpoint::send(std::string message)
{
    websocketpp::lib::error_code ec;
    g_wsEndPoint.send(g_wsClientConnection->get_hdl(), message, websocketpp::frame::opcode::text, ec);
    if (ec)
    {
        std::cout << "> Error sending message: " << ec.message() << std::endl;
        return;
    }
    g_wsClientConnection->record_send_message(message);
}

void websocket_endpoint::show()
{
    std::cout << *g_wsClientConnection << std::endl;
}