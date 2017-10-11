#include "Navigation.h"

#include <windows.h>
#include <stdio.h>

extern "C"
{
	__declspec(dllexport) int CalculateStraightPath(unsigned int mapId, XYZ start, XYZ end)
	{
		return Navigation::GetInstance()->CalculateStraightPath(mapId, start, end);
	}

	__declspec(dllexport) int CalculateSmoothPath(unsigned int mapId, XYZ start, XYZ end)
	{
		return Navigation::GetInstance()->CalculateSmoothPath(mapId, start, end);
	}

	__declspec(dllexport) void GetPathArray(XYZ* path, int length)
	{
		Navigation::GetInstance()->GetPath(path, length);
	}
};

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	Navigation* navigation = Navigation::GetInstance();
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
			navigation->Initialize();
			break;

		case DLL_PROCESS_DETACH:
			navigation->Release();
			break;

		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
			break;
	}

	return TRUE;
}
