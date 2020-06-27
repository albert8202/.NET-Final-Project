// ATLSimpleObject.cpp: CATLSimpleObject 的实现

#include "pch.h"
#include "ATLSimpleObject.h"


// CATLSimpleObject

HRESULT STDMETHODCALLTYPE CATLSimpleObject::changeStatus(int status, int* newStatus)
{
	*newStatus = status + 1;
	return S_OK;
}