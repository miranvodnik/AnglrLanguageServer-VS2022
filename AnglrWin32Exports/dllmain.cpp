// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

BOOL APIENTRY DllMain(HMODULE hModule, DWORD, LPVOID)

{
	DisableThreadLibraryCalls(hModule);
	return	TRUE;
}

extern "C"
{
	__declspec(dllexport) DWORD GetParentProcessId(DWORD procId)
	{
		HANDLE	hProcessSnap;
		PROCESSENTRY32	pe32;
		DWORD	id = 0;

		if ((hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0)) != INVALID_HANDLE_VALUE)
		{
			pe32.dwSize = sizeof(PROCESSENTRY32);

			if (Process32First(hProcessSnap, &pe32) != FALSE)

				do
				{
					if (procId != pe32.th32ProcessID)
						continue;
					id = pe32.th32ParentProcessID;
					break;
				} while (Process32Next(hProcessSnap, &pe32) != FALSE);

				CloseHandle(hProcessSnap);
		}

		return id;
	}

	__declspec(dllexport) DWORD CreateRegexClass(char** regexTextArray, int regexTextArraySize)
	{
		string regexText = "^(?:";
		for (int i = 0; i < regexTextArraySize; ++i)
		{
			char* text = regexTextArray[i];
			try
			{
				regex r(text);
				if (i > 0)
					regexText += "|";
				regexText += "(";
				regexText += text;
				regexText += ")";
			}
			catch (...)
			{
				cout << "regex (" << text << ") failed" << endl;
			}
		}
		regexText += ")";
		cout << "compound regex = " << regexText.c_str() << endl;
		return 0;
	}
}
