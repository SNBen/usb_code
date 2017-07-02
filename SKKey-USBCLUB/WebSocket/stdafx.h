// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>

#include <string>


long long GetCurrentStamp64();
std::string TSM_Init(std::string type, std::string content, bool request, bool async);

#pragma comment(lib,"jsoncpp.lib")

// TODO: reference additional headers your program requires here
