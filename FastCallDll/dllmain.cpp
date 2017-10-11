// dllmain.cpp : Definiert den Einstiegspunkt für die DLL-Anwendung.
#include "stdafx.h"
#include <windows.h>
#include <sstream>
#include <tlhelp32.h>

using namespace std;

#include <stdio.h>

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		
		break;

	case DLL_PROCESS_DETACH:
		
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}

	return TRUE;
}

//template <typename T>
//string NumberToString(T Number)
//{
//	ostringstream ss;
//	ss << Number;
//	return ss.str();
//}
//
//template <typename T>
//string ptrToString(T* Number)
//{
//	const void * address = static_cast<const void*>(this);
//	std::stringstream ss;
//	ss << address;
//	std::string name = ss.str();
//}
//
//
//void str_replace(string &s, const string &search, const string &replace)
//{
//	for (size_t pos = 0;; pos += replace.length())
//	{
//		pos = s.find(search, pos);
//		if (pos == string::npos) break;
//
//		s.erase(pos, search.length());
//		s.insert(pos, replace);
//	}
//}

//char* strToChar(string str)
//{
//	return strcpy((char*)malloc(str.length() + 1), str.c_str());
//}

//typedef bool (WINAPI* Module32First)(HANDLE, LPMODULEENTRY32*);
//typedef bool (WINAPI* Module32Next)(HANDLE, LPMODULEENTRY32*);


extern "C"
{
	typedef struct XYZXYZ { float X1; float Y1; float Z1; float X2; float Y2; float Z2; };
	typedef struct Intersection { float X; float Y; float Z; float R; };

	void __declspec(dllexport) __stdcall _DoString(char* code)
	{
		typedef void __fastcall func(char* code, int zero);
		func* f = (func*)0x00704CD0;
		f(code, 0);
		return;

	}

	//006F3510

	int __declspec(dllexport) __stdcall _LuaIsString(unsigned int LuaStatePtr, int number)
	{
		typedef int __fastcall func(unsigned int LuaStatePtr, int number);
		func* f = (func*)0x006F3510;
		return f(LuaStatePtr, number);
	}

	int __declspec(dllexport) __stdcall _LuaIsNumber(unsigned int LuaStatePtr, int number)
	{
		typedef int __fastcall func(unsigned int LuaStatePtr, int number);
		func* f = (func*)0x006F34D0;
		return f(LuaStatePtr, number);
	}

	void __declspec(dllexport) __stdcall _RegFunc(char* funcName, unsigned int funcPtr)
	{
		typedef void __fastcall func(char* funcName, unsigned int funcPtr);
		func* f = (func*)0x00704120;
		f(funcName, funcPtr);
	}

	void __declspec(dllexport) __stdcall _UnregFunc(char* funcName, unsigned int funcPtr)
	{
		typedef void __fastcall func(char* funcName, unsigned int funcPtr);
		func* f = (func*)0x00704160;
		f(funcName, funcPtr);
	}

	double __declspec(dllexport) __stdcall _LuaToNumber(unsigned int LuaStatePtr, int number)
	{
		typedef double __fastcall func(unsigned int LuaStatePtr, int number);
		func* f = (func*)0x006F3620;
		return f(LuaStatePtr, number);
	}

	unsigned int __declspec(dllexport) __stdcall _LuaToString(unsigned int LuaStatePtr, int number)
	{
		typedef unsigned int __fastcall func(unsigned int LuaStatePtr, int number);
		func* f = (func*)0x006F3690;
		return f(LuaStatePtr, number);
	}

	/*bool __declspec(dllexport) __stdcall _Module32First(HANDLE hwnd, MODULEENTRY32 module)
	{
		
		return Module32First(hwnd, &module);
	}

	bool __declspec(dllexport) __stdcall _Module32Next(HANDLE hwnd, MODULEENTRY32 module)
	{
		return Module32Next(hwnd, &module);
	}*/

	unsigned int __declspec(dllexport) __stdcall _GetText(char* varName)
	{
		typedef unsigned int __fastcall func(char* varName, unsigned int nonSense, int zero);
		func* f = (func*)0x703BF0;
		return f(varName, 0xFFFFFFFF, 0);
	}

	void __declspec(dllexport) __stdcall _EnumVisibleObjects(unsigned int callback, int filter)
	{
		typedef void __fastcall func(unsigned int callback, int filter);
		func* f = (func*)0x00468380;
		f(callback, filter);
	}

	BYTE __declspec(dllexport) __stdcall _Intersect(XYZXYZ* points, float* distance, Intersection* intersection, int flags)
	{
		typedef BYTE __fastcall func(struct XYZXYZ* addrPoints, float* addrDistance, struct Intersection* addrIntersection, int flags);
		func* f = (func*)0x6aa160;
		return f(points, distance, intersection, flags);
	}

	void __declspec(dllexport) __stdcall _CastSpell(int SpellId)
	{
		typedef void __fastcall func(int SpellId, int zero, int _zero, int __zero);
		func* f = (func*)0x6E5A90;
		f(SpellId, 0, 0, 0);
	}

	void __declspec(dllexport) __stdcall _UseItem(unsigned int itemPtr, unsigned int useItemPtr)
	{
		typedef void __fastcall func(unsigned int itemPtr, unsigned int useItemPtr);
		func* f = (func*)0x005D8D00;
		f(itemPtr, useItemPtr);
	}
}
