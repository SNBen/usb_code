// WebSocket.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <sstream>

#include <json/value.h>
#include <json/writer.h>

#include "websocket_endpoint.h"
#include <boost/date_time/posix_time/posix_time.hpp>
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

void Test()
{
    using namespace boost::gregorian;
    using namespace boost::posix_time;
    Json::FastWriter styled_writer;
    ptime now = posix_time::second_clock::universal_time();
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

    std::string strInit(R"({"id":1498960459621,"type":"initDevice","content":"{\"GUID\":\"{B69392DF-209B-4102-819B-3C34D9969B86}\",\"CompanyName\":\"\",\"ACTION\":\"1\",\"TaxCodeList\":[\"91500000747150540A\",\"110102681953105\"],\"TIME\":\"7/2/2017 9:54:23 AM\"}","parameters":{},"createTime":1498960463676,"request":true,"async":false})");
    LOG_MODULE_INFO("%s", strInit.c_str());
    LOG_MODULE_INFO("%s", styled_writer.write(initDevice_).c_str());
}

int main(int argc,char** argv)
{
    bool done = false;
    std::string input;
    websocket_endpoint endpoint;

    endpoint.connect("ws://server.ngrok.cc:7088/websocket");
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

