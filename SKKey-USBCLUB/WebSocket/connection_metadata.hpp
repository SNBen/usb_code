#ifndef _CONNECTION_METADATA_H_A0CBB61C_CF90_41C0_B750_5838FC2CF383_
#define _CONNECTION_METADATA_H_A0CBB61C_CF90_41C0_B750_5838FC2CF383_
#pragma once
#include <websocketpp/client.hpp>
#include <LogExt.h>
#include <json/writer.h>
#include <json/json.h>

#include <boost/date_time/posix_time/posix_time.hpp>

typedef websocketpp::client<websocketpp::config::asio_client> ws_client;

class connection_metadata
{
public:
    typedef websocketpp::lib::shared_ptr<connection_metadata> ptr;

    connection_metadata(websocketpp::connection_hdl hdl, std::string uri)
        : m_hdl(hdl)
        , m_status("connecting")
        , m_uri(uri)
        , m_server("N/A") {}

    void on_open(ws_client * client, websocketpp::connection_hdl hdl)
    {
        m_status = "Open";
        ws_client::connection_ptr con = client->get_con_from_hdl(hdl);
        m_server = con->get_response_header("Server");
        initdevice(client, hdl);
    }

    void on_fail(ws_client * client, websocketpp::connection_hdl hdl)
    {
        m_status = "Failed";
        ws_client::connection_ptr  con = client->get_con_from_hdl(hdl);
        m_server = con->get_response_header("Server");
        m_error_reason = con->get_ec().message();
    }

    void on_close(ws_client * client, websocketpp::connection_hdl hdl)
    {
        m_status = "Closed";
        ws_client::connection_ptr con = client->get_con_from_hdl(hdl);
        std::stringstream s;
        s << "close code:" << con->get_remote_close_code() << "("
            << websocketpp::close::status::get_string(con->get_remote_close_code())
            << "),close reason: " << con->get_remote_close_reason();
        m_error_reason = s.str();
    }

    void on_message(websocketpp::connection_hdl, ws_client::message_ptr msg)
    {
        if (msg->get_opcode() == websocketpp::frame::opcode::text)
        {
            m_messages.push_back("<<" + msg->get_payload());
        }
        else
        {
            m_messages.push_back("<<" + websocketpp::utility::to_hex(msg->get_payload()));
        }
    }

    websocketpp::connection_hdl get_hdl() const { return m_hdl; }

    std::string get_status() const {
        return m_status;
    }

    std::string get_uri() const {
        return m_uri;
    }

    void record_send_message(std::string message)
    {
        m_messages.push_back(">> " + message);
    }

    void initdevice(ws_client * client, websocketpp::connection_hdl hdl)
    {
        using namespace boost::posix_time;
        using namespace boost::gregorian;
        Json::FastWriter styled_writer;
        ptime now = second_clock::universal_time();
        std::cout << GetCurrentStamp64() << std::endl;

        Json::Value content_;
        content_["GUID"] = "B69392DF-209B-4102-819B-3C34D9969B86";
        content_["CompanyName"] = "";
        content_["TaxCodeList"].append("91500000747150540A");
        content_["TaxCodeList"].append("110102681953105");
        content_["TIME"] = to_iso_extended_string(now);

        Json::Value initDevice_;
        initDevice_["id"] = GetCurrentStamp64();
        initDevice_["type"] = "initDevice";
        initDevice_["content"] = styled_writer.write(content_);
        initDevice_["parameters"] = Json::Value::null;
        initDevice_["request"] = Json::Value(true);
        initDevice_["async"] = Json::Value(false);
        initDevice_["createTime"] = GetCurrentStamp64();

        LOG_MODULE_INFO("%s", styled_writer.write(initDevice_).c_str());
        websocketpp::lib::error_code ec;
        client->send(hdl, styled_writer.write(initDevice_), websocketpp::frame::opcode::text, ec);
        
        if (ec)
        {
            LOG_MODULE_ERR("> Error sending message: %s", ec.message().c_str());
            return;
        }
    }

    friend std::ostream & operator<< (std::ostream & out, connection_metadata const & data);
protected:
private:
    websocketpp::connection_hdl m_hdl;
    std::string m_status;
    std::string m_uri;
    std::string m_server;
    std::string m_error_reason;
    std::vector<std::string> m_messages;
};

std::ostream & operator<< (std::ostream & out, connection_metadata const & data)
{
    out << "> URI: " << data.m_uri << "\n"
        << "> Status: " << data.m_status << "\n"
        << "> Remote Server: " << (data.m_server.empty() ? "None Specified" : data.m_server) << "\n"
        << "> Error/close reason: " << (data.m_error_reason.empty() ? "N/A" : data.m_error_reason) << "\n"
        << "> Message Processed: (" << data.m_messages.size() << ") \n";

    std::vector<std::string>::const_iterator it;
    for (it = data.m_messages.begin(); it != data.m_messages.end(); ++it)
    {
        out << *it << "\n";
    }

    return out;
}


#endif // _CONNECTION_METADATA_H_A0CBB61C_CF90_41C0_B750_5838FC2CF383_
