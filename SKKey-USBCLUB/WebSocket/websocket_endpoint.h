#ifndef _WEBSOCKET_ENDPOINT_H_DC9FCCBA_70C5_49F1_899E_626BEE66DA03_
#define _WEBSOCKET_ENDPOINT_H_DC9FCCBA_70C5_49F1_899E_626BEE66DA03_
#pragma once

#include <string>
class websocket_endpoint
{
public:
    websocket_endpoint();
    ~websocket_endpoint();

    int connect(std::string const &uri);

    void close();

    void send(std::string message);

    void show();
};


#endif // _WEBSOCKET_ENDPOINT_H_DC9FCCBA_70C5_49F1_899E_626BEE66DA03_
