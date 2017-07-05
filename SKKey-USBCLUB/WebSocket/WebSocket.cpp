// WebSocket.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <sstream>

#include <json/value.h>
#include <json/writer.h>
#include <json/reader.h>

#include "websocket_endpoint.h"
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/thread.hpp>
#include <boost/chrono.hpp>
#include <LogExt.h>

using namespace std;
using namespace boost;

long long GetCurrentStamp64()
{
    using namespace boost::posix_time;
    ptime epoch(boost::gregorian::date(1970, boost::gregorian::Jan, 1));
    time_duration time_from_epoch =
        second_clock::universal_time() - epoch;

    return time_from_epoch.total_seconds();
}

std::string TSM_Init(std::string type , std::string content,bool request,bool async)
{
    Json::FastWriter styled_writer;
    Json::Value tsm;
    tsm["id"] = GetCurrentStamp64();
    tsm["type"] = type;
    tsm["content"] = content.empty()? Json::Value::null : content;
    tsm["parameters"] = Json::Value::null;
    tsm["request"] = Json::Value(request);
    tsm["async"] = Json::Value(async);
    tsm["createTime"] = GetCurrentStamp64();
    return styled_writer.write(tsm);
}

void Test()
{
    using namespace boost::gregorian;
    using namespace boost::posix_time;
    
    Json::FastWriter styled_writer;
    Json::Value content_;
    content_["GUID"] = "B69392DF-209B-4102-819B-3C34D9969B86";
    content_["CompanyName"] = "";
    content_["TaxCodeList"].append("91500000747150540A");
    content_["TaxCodeList"].append("110102681953105");
    content_["TIME"] = to_iso_extended_string(second_clock::universal_time());

    std::string Init = TSM_Init("initDevice", styled_writer.write(content_) ,true,false);
    std::string keepalive =  TSM_Init("keepalive", "", true, false);

    LOG_MODULE_INFO("%s", Init.c_str());
    LOG_MODULE_INFO("%s", keepalive.c_str());
}

void Test2()
{
    std::string strMessage(R"-({"id":12,"type":"pushTask","content":"{\"jxKhdwId\":451285,\"jxSqzhId\":3203,\"nsrsbh\":\"91500000747150540A\",\"rwlx\":\"2\",\"zt\":\"1\",\"errno\":\"903\",\"sbyy\":\"\",\"createTime\":\"2017-06-30 22:08:15\",\"beginTime\":\"2017-07-02 14:59:37\",\"endTime\":\"2017-07-01 21:34:27\",\"param\":\"{\\\"nsrsbh\\\":\\\"91500000747150540A\\\",\\\"password\\\":\\\"88888888\\\",\\\"url\\\":\\\"https://fpdk.bjsat.gov.cn\\\",\\\"ymbb\\\":\\\"3.0.09\\\"}\",\"result\":\"\",\"jxExchangeLicense\":\"B69392DF-209B-4102-819B-3C34D9969B85\"}", "parameters":{}, "createTime" : 1498978807430, "request" : true, "async" : false})-");
    std::stringstream ss(strMessage);
    Json::Reader _reader;
    Json::Value tsm;
    _reader.parse(ss, tsm);
    LOG_MODULE_INFO("%d", tsm["id"]);
    LOG_MODULE_INFO("%s", tsm["type"]);
    LOG_MODULE_INFO("%s", tsm["content"]);
}

int main(int argc,char** argv)
{
    Test2();

    bool done = false;
    std::string input;
    websocket_endpoint endpoint;

    endpoint.connect("ws://server.ngrok.cc:7088/websocket");

    using namespace boost;
    boost::thread keepalive([&]() {
        this_thread::sleep_for(chrono::seconds(5));
        do 
        {
        	std::string Init = TSM_Init("keepalive", "", true, false);
            endpoint.send(Init);
            this_thread::sleep_for(chrono::seconds(60));
        } while (true);
    });
    keepalive.detach();

    while (!done)
    {
        std::cout << "Enter Command: ";
        std::getline(std::cin, input);

        if (input == "quit")
        {
            done = true;
        }

        else if (input.substr(0,4) == "send")
        {
            std::stringstream ss(input);
            std::string cmd;
            std::string message;
            ss >> cmd;
            std::getline(ss, message);
            endpoint.send(message);
        }
        else if(input.substr(0,4) == "show")
        {
            endpoint.show();
        }
        else
        {
            std::cout << "> Unrecognized Command" << std::endl;
        }
    }

    endpoint.close();
    return 0;
}

