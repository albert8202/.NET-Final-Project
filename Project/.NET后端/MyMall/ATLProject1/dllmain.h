// dllmain.h: 模块类的声明。

class CATLProject1Module : public ATL::CAtlDllModuleT< CATLProject1Module >
{
public :
	DECLARE_LIBID(LIBID_ATLProject1Lib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_ATLPROJECT1, "{ef542178-5844-4cb5-9c83-358a8e84e722}")
};

extern class CATLProject1Module _AtlModule;
