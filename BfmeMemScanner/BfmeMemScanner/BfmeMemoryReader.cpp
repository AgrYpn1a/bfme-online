#include <Windows.h>
#include <iostream>
#include <string>
#include <vector>
#include <tlhelp32.h>
#include <TlHelp32.h>

char* GetAddressOfData(DWORD pid, const char* data, size_t len)
{
	HANDLE process = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, FALSE, pid);
	if (process)
	{
		SYSTEM_INFO si;
		GetSystemInfo(&si);

		std::cout << "Opening process..." << std::endl;

		MEMORY_BASIC_INFORMATION info;
		std::vector<char> chunk;
		char* p = 0;
		while (p < si.lpMaximumApplicationAddress)
		{
			if (VirtualQueryEx(process, p, &info, sizeof(info)) == sizeof(info))
			{
				p = (char*)info.BaseAddress;
				chunk.resize(info.RegionSize);
				SIZE_T bytesRead;
				if (ReadProcessMemory(process, p, &chunk[0], info.RegionSize, &bytesRead))
				{
					for (size_t i = 0; i < (bytesRead - len); ++i)
					{
						if (memcmp(data, &chunk[i], len) == 0)
						{
							return (char*)p + i;
						}
					}
				}
				p += info.RegionSize;
			}
		}
	}
	return 0;
}

uintptr_t GetModuleBaseAddress(DWORD procId, const wchar_t* modName)
{
	uintptr_t modBaseAddr = 0;
	HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32, procId);
	if (hSnap != INVALID_HANDLE_VALUE)
	{
		MODULEENTRY32 modEntry;
		modEntry.dwSize = sizeof(modEntry);
		if (Module32First(hSnap, &modEntry))
		{
			do
			{
				if (!_wcsicmp(modEntry.szModule, modName))
				{
					modBaseAddr = (uintptr_t)modEntry.modBaseAddr;
					break;
				}
			} while (Module32Next(hSnap, &modEntry));
		}
	}
	CloseHandle(hSnap);
	return modBaseAddr;
}

DWORD FindProcessId(LPCTSTR ProcessName) // non-conflicting function name
{
	PROCESSENTRY32 pt;
	HANDLE hsnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	pt.dwSize = sizeof(PROCESSENTRY32);
	if (Process32First(hsnap, &pt)) { // must call this first
		do {
			if (!lstrcmpi(pt.szExeFile, ProcessName)) {
				CloseHandle(hsnap);
				return pt.th32ProcessID;
			}
		} while (Process32Next(hsnap, &pt));
	}
	CloseHandle(hsnap); // close handle on failure
	return 0;
}

#define BASE 0x00ef0898
#define OFFSET_1 0x54
#define OFFSET_2 0x00
#define OFFSET_3 0x08

int main()
{

	std::cout << "Starting scan... " << std::endl;

	// Get pid 
	DWORD ProcessId = FindProcessId(TEXT("game.dat"));

	// Get process handle
	HANDLE pHandle = OpenProcess(PROCESS_VM_READ, FALSE, ProcessId);

	// Get process address
	DWORD game_dat = (DWORD)GetModuleBaseAddress(ProcessId, L"game.dat");

	if (pHandle)
	{
		DWORD BasePointer, Offset1, Offset2, Offset3, Offset4, ResultAddr;
		BasePointer = ((DWORD)game_dat + BASE);

		ReadProcessMemory(pHandle, (LPCVOID)BasePointer, &Offset1, 4, NULL);
		//printf("Base: 0x%08X \n\n", Offset1);

		ReadProcessMemory(pHandle, (LPCVOID)((DWORD)Offset1 + OFFSET_1), &Offset2, 4, NULL);
		//printf("Offset1: 0x%08X \n\n", Offset2);

		ReadProcessMemory(pHandle, (LPCVOID)((DWORD)Offset2), &Offset3, 4, NULL);
		//printf("Offset2: 0x%08X \n\n", Offset3);

		ResultAddr = Offset3 + OFFSET_3;
		printf("Result Address: 0x%08X \n\n", ResultAddr);

		char data[1024];
		ReadProcessMemory(pHandle, (LPCVOID)ResultAddr, &data, sizeof(data), NULL);

		printf("%s", data);

		CloseHandle(pHandle);
	}

	std::cout << std::endl;
	system("PAUSE");

	return 0;
}