#pragma once

using namespace System;

namespace Warnings {
	public ref class Class1
	{
		// TODO: 在此处为此类添加方法。
	public:
		static String^ UserExists = "该手机号已被注册";
		static String^ UserNotExist = "用户不存在";
		static String^ LoginErroer = "用户名或者密码错误";
		static String^ AlbumNotExist = "专辑不存在";
		static String^ AlbumOutOfStock = "卖光了";
		static String^ ImpossibleError = "不可能出现的错误";

	};
}
