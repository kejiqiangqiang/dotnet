/*
	版权所有 2009-2016 荆门泽优软件有限公司
	保留所有权利
	官方网站：http://www.ncmem.com
	官方博客：http://www.cnblogs.com/xproer
	产品首页：http://www.ncmem.com/webplug/http-uploader2/
	在线演示：http://www.ncmem.com/products/http-uploader/demo2/index.html
	开发文档：http://www.cnblogs.com/xproer/archive/2011/03/15/1984950.html
	升级日志：http://www.cnblogs.com/xproer/archive/2011/03/15/1985091.html
	示例下载：http://www.ncmem.com/download/HttpUploader/asp.net/HttpUploader-demo.rar
	文档下载：http://www.ncmem.com/download/HttpUploader/HttpUploader-doc.rar
	联系邮箱：1085617561@qq.com
	联系QQ：1085617561
	更新记录：
		2009-11-05 创建
		2014-02-28 使用jquery优化代码并增强兼容性。
*/

//删除元素值
Array.prototype.remove = function(val)
{
	for (var i = 0, n = 0; i < this.length; i++)
	{
		if (this[i] != val)
		{
			this[n++] = this[i]
		}
	}
	this.length -= 1
}

var HttpUploaderErrorCode = {
	  "0": "发送数据错误"
	, "1": "接收数据错误"
	, "2": "域名未授权"
	, "3": "公司未授权"
	, "4": "访问本地文件错误"
    //md5
	, "200": "无打打开文件"
	, "201": "文件大小为0"
};

function HttpUploaderMgr()
{
    var _this = this;
	//http://www.ncmem.com/
	this.Domain = "http://" + document.location.host;
	
	this.Config = {
		"EncodeType"		: "gb2312"//UTF-8,GB2312。文件名称编码方式，与web.config-requestEncoding配置对应
		, "Version"			: "2,5,53,40765"
		, "Company"			: "荆门泽优软件有限公司"
		, "License"			: ""
		, "Debug"			: false//调试开关
		, "LogFile"			: "C:\\log.txt"//日志文件路径
		, "FileFilter"		: "*"//文件类型。所有类型：*。自定义类型：jpg,png,gif,bmp
	    //字节计算器：http://www.beesky.com/newsite/bit_byte.htm
	    //超过10MB建议选择HttpUploader6：http://www.ncmem.com/webplug/http-uploader6/index.asp
		, "FileSizeLimit"	: "10485760"//允许上传的文件大小，默认10MB
		, "AllowMultiSelect": true//多选开关。true:开启多选。false:关闭多选
		, "CryptoType"      : "md5"//文件校验方式：md5,crc,sha1
		, "InitDir"			: ""//初始路径。示例：D:\\Soft
		, "Compress"		: ""//是否开启压缩。值：zip,gzip
		, "AppPath"			: ""//网站虚拟目录名称。子文件夹 web
		, "FileFieldName"	: "file"//文件字段名称
		, "Cookie"	        : ""//cookie
        , "Authenticate"    : {type:"ntlm",name:"",pass:""}//域环境信息
		, "PostUrl"			: "http://localhost:4854/asp.net/upload.aspx"
        //x86
		, "ClsidDroper"		: "4D2454F8-EB25-465f-B867-C2A3E9F3D4B4"
		, "ClsidPartition"	: "6F3EB4AF-FC9C-4570-A686-88B4B427C6FE"
		, "CabPath"			: "http://www.ncmem.com/download/HttpUploader2/HttpUploader.cab"
		//x64
		, "ClsidDroper64"	: "C9388115-887C-4d64-B175-F8F1AA5437BF"
		, "ClsidPartition64": "3AFFCB6D-55ED-4ada-A1EC-D7D87BA29E51"
		, "CabPath64"		: "http://www.ncmem.com/download/HttpUploader2/HttpUploader64.cab"
		//Firefox
		, "XpiType"		    : "application/npHttpUp2"
		, "XpiPath"	        : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.xpi"
		//Chrome
		, "CrxName"			: "npHttpUp2"
		, "CrxType"		    : "application/npHttpUp2"
		, "CrxPath"	        : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.crx"
		, "ExePath"			: "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.exe"
	    //Chrome 45
        , "NatHostName"     : "com.xproer.up2"//
	    , "NatPath"		    : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.nat.crx"
	    , "ExePath"		    : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.exe"
	};

	this.ActiveX = {
		  "Droper"      : "Xproer.HttpDroper2"
		, "Partition"   : "Xproer.HttpPartition2"
		//64bit
		, "Droper64"	: "Xproer.HttpDroper2x64"
		, "Partition64"	: "Xproer.HttpPartition2x64"
	};
	
	//附加参数
	this.Fields = {
		 "uname": "test"
		,"upass": "123456"
	};

	this.filesMap = new Object();//文件对象,(id,json),(id,json)
	this.Droper = null;
	this.parter = null;
	this.npapi = false;//
	this.uiToolBar = null;//jquery object
	this.uiToolBarBk = null;
	this.nat_loaded = false;
	
	this.AddFile = function(pathLoc)
	{
	    this.browser.addFile(pathLoc);
	};
	this.postAll = function () { this.browser.post(); };
	this.part_files = function (ret)
	{
	    if (ret.files == null) return;
	    for (var i = 0, l = ret.files.length; i < l; ++i)
	    {
	        var f = ret.files[i];
	        this.addFileLoc(f);
	        //jQuery.extend(f, this.Fields);//附加字段
	        //jQuery.extend(f, { name: "test" });
	        this.browser.updateFile(f);//增加扩展信息
	    }
	};
	this.drag_enter = function(arg){}
	this.drag_leave = function(arg){}
	this.drop_files = function (ret)
	{
	    this.part_files(ret);
	};
	this.post_complete = function (ret)
	{
	    var f = this.filesMap[ret.id];
	    f.postComplete();
		//服务器返回信息
	    //alert(ret.res);
	    f.remove();
	    delete f;
	    this.filesMap[ret.id] = null;
	};
	this.post_error = function (ret)
	{
	    if (ret.value == "12")
	    {
	        this.setupTip();
	        return;
	    }
	    if (ret.value == "2")
	    {
	        alert(HttpUploaderErrorCode[ret.value]);
	        return;
	    }
	    var f = this.filesMap[ret.id];
	    f.postError(ret);
	};
	this.post_process = function (ret)
	{
	    var f = this.filesMap[ret.id];
	    f.postProcess(ret);
	};
	this.queue_complete = function (ret)
	{
	};
	this.plugin_loaded = function (arg)
	{
	    this.nat_loaded = true;
	    this.uiToolBar.empty();
	    this.uiToolBar.replaceWith(this.uiToolBarBk);
	    //this.uiToolBar.show();
	}
	this.stateChanged = function (str)
	{
	    var ret = JSON.parse(str);
        if      (ret.name == "part_files") _this.part_files(ret);
        else if (ret.name == "drag_enter") _this.drag_enter(ret);
        else if (ret.name == "drag_leave") _this.drag_leave(ret);
        else if (ret.name == "drop_files") _this.drop_files(ret);
        else if (ret.name == "post_complete") _this.post_complete(ret);
        else if (ret.name == "post_process") _this.post_process(ret);
        else if (ret.name == "post_error") _this.post_error(ret);
        else if (ret.name == "queue_complete") _this.queue_complete(ret);
        else if (ret.name == "plugin_loaded") _this.plugin_loaded(ret);
	};

	this.browser = {
	      entID: "Uploader2Event"
        , check: function ()
        {
            try
            {
                return this.GetVersion() != "0";
            }
            catch (e) { return false; }
	      }
	    , checkFF: function ()
	    {
	        var mimetype = navigator.mimeTypes;
	        if (typeof mimetype == "object" && mimetype.length)
	        {
	            for (var i = 0; i < mimetype.length; i++)
	            {
	                if (mimetype[i].type == _this.Config["XpiType"].toLowerCase())
	                {
	                    return mimetype[i].enabledPlugin;
	                }
	            }
	        }
	        else
	        {
	            mimetype = [_this.Config["XpiType"]];
	        }
	        if (mimetype)
	        {
	            return mimetype.enabledPlugin;
	        }
	        return false;
	    }
        , checkNat: function ()
        {
            return _this.nat_loaded;
        }
        , GetVersion: function ()
        {
            var v = "0";
            try
            {
                v = _this.parter.Version;
                if (v == undefined) v = "0";
            }
            catch (e) { }
            return v;
        }
        , NeedUpdate: function ()
        {
            return this.GetVersion() != _this.Config["Version"];
        }
        , setup: function ()
        {
            //文件上传控件
            var acx = "";
            //文件夹选择控件
            acx += '<object classid="clsid:' + _this.Config["ClsidPartition"] + '"';
            acx += ' codebase="' + _this.Config["CabPath"] + '" width="1" height="1" ></object>';

            $("body").append(acx);
        }
        , setupFF: function ()
        {
            var xpi = new Object();
            xpi["Calendar"] = _this.Config["XpiPath"];
            InstallTrigger.install(xpi, function (name, result) { });
        }
        , addFile: function (pathLoc)
        {
            var par = { name: "add_file", path: pathLoc, config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
        }
        , updateFile: function (inf)
        {
            var par = { name: "update_file", fields: inf };
            this.postMessage(par);
        }
        , openFile: function ()
        {
            var par = { name: "open_file", config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
        }
        , openFolder: function ()
        {
            var par = { name: "open_folder", config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
        }
        , pasteFiles: function ()
        {
            var par = { name: "paste_file", config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
        }
        , init: function ()
        {
            if (!this.check()) { return;};
            var par = {name:"init",config:_this.Config,fields:_this.Fields};
            this.postMessage(par);
            _this.parter.StateChanged = _this.stateChanged;
        }
        , initNat: function ()
        {
            _this.uiToolBarBk = _this.uiToolBar.clone(true);
            this.exitEvent();
            document.addEventListener('Uploader2EventCallBack', function (evt)
            {
                _this.stateChanged(JSON.stringify(evt.detail));
            });
        }
        , post: function ()
        {
            var par = { name: "queue_post", config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
        }
        , stop: function (json/*id*/) { this.postMessage(json); }
        , exit: function ()
        {
            var par = { name: 'exit' };
            var evt = document.createEvent("CustomEvent");
            evt.initCustomEvent(this.entID, true, false, par);
            document.dispatchEvent(evt);
        }
        , exitEvent: function ()
        {
            var obj = this;
            $(window).bind("beforeunload", function () { obj.exit(); });
        }
        , postMessage: function (obj) { _this.parter.postMessage(JSON.stringify(obj)); }
        , postMessageNat: function (json)
        {
            var evt = document.createEvent("CustomEvent");
            evt.initCustomEvent(this.entID, true, false, json);
            document.dispatchEvent(evt);
        }
	};

	var browserName = navigator.userAgent.toLowerCase();
	this.ie = browserName.indexOf("msie") > 0;
    //IE11
	this.ie = this.ie ? this.ie : browserName.search(/(msie\s|trident.*rv:)([\w.]+)/) != -1;
	this.firefox = browserName.indexOf("firefox") > 0;
	this.chrome = browserName.indexOf("chrome") > 0;
	this.chrome45 = false;
	this.chrVer = navigator.appVersion.match(/Chrome\/(\d+)/);

    //浏览器环境检查
	if (_this.ie)
	{
	    //Win64
	    if (window.navigator.platform == "Win64")
	    {
	        this.Config["CabPath"] = this.Config["CabPath64"];
	        this.Config["ClsidDroper"] = this.Config["ClsidDroper64"];
	        this.Config["ClsidPartition"] = this.Config["ClsidPartition64"];
	        //
	        this.ActiveX["Partition"] = this.ActiveX["Partition64"];
	    }
	    //if (!_this.Browser.Check()) { window.open(_this.Config["SetupPath"], "_blank"); /*_this.Browser.Setup();*/ } 
	}
	else if (_this.firefox)
	{
	    this.npapi = true;
	    this.browser.check = this.browser.checkFF;
	    this.browser.setup = this.browser.setupFF;
	} //Chrome
	else if (_this.chrome)
	{
	    this.npapi = true;
	    _this.Config["XpiPath"] = _this.Config["CrxPath"];
	    _this.Config["XpiType"] = _this.Config["CrxType"];
	    this.browser.check = this.browser.checkFF;
	    //44+版本使用Native Message
	    if (parseInt(this.chrVer[1]) >= 44)
	    {
	        if (!this.browser.checkFF())//仍然支持npapi
	        {
	            this.npapi = false;
	            _this.chrome45 = true;//
	            this.browser.check = this.browser.checkNat;
	            this.browser.init = this.browser.initNat;
	            this.browser.postMessage = this.browser.postMessageNat;
	        }
	    }
	}

	this.GetHtml = function ()
	{
	    //加载拖拽控件
	    var acx = '<object name="objDroper" classid="clsid:' + this.Config["ClsidDroper"] + '"';
	    acx += ' codebase="' + this.Config["CabPath"] + '#version=' + this.Config["Version"] + '" width="192" height="192" >';
	    acx += '</object>';

	    var plugin_html = '<object name="parter" id="objPartition" classid="clsid:' + this.Config["ClsidPartition"] + '"';
	    plugin_html += ' codebase="' + this.Config["CabPath"] + '#version=' + this.Config["Version"] + '" width="1" height="1" ></object>';

	    if (this.firefox||this.chrome && !this.chrome45)
	    {
	        plugin_html = '<embed name="parter" type="' + this.Config["XpiType"] + '" pluginspage="' + this.Config["XpiPath"] + '" width="1" height="1"/>';
	    }
	    acx += plugin_html;
	    //自动安装CAB
	    //acx += '<div style="display:none">';
	    //文件夹选择控件
	    //acx += '<object name="part" id="objPartition" classid="clsid:' + this.Config["ClsidPartition"] + '"';
	    //acx += ' codebase="' + this.Config["CabPath"] + '#version=' + this.Config["Version"] + '" width="1" height="1" ></object>';
	    //acx += '</div>';
	    //上传列表项模板
	    acx += '<div name="tmpItem" class="UploaderItem" id="UploaderTemplate">\
					<div class="UploaderItemLeft">\
						<div class="FileInfo">\
							<div name="fileName" class="FileName top-space">HttpUploader程序开发.pdf</div>\
							<div name="fileSize" class="FileSize" child="1">100.23MB</div>\
						</div>\
						<div class="ProcessBorder top-space"><div name="process" class="Process"></div></div>\
						<div name="msg" class="PostInf top-space">已上传:15.3MB 速度:20KB/S 剩余时间:10:02:00</div>\
					</div>\
					<div class="UploaderItemRight">\
						<div class="BtnInfo"><span name="btnCancel" class="Btn">取消</span>&nbsp;<span name="btnDel" class="Btn hide">删除</span></div>\
						<div name="percent" class="ProcessNum">35%</div>\
					</div>';
	    acx += '</div>';
	    //分隔线
	    acx += '<div name="spliter" class="spliter" id="FilePostLine"></div>';
	    //上传列表
	    acx += '<div name="pnlUpload" id="UploaderPanel">\
		            <div class="header">上传文件</div>\
		            <div name="toolbar" class="toolbar">\
		                <input name="btnAddFiles" id="btnAddFiles" type="button" value="选择多个文件" />\
		                <input name="btnAddFolder" id="btnAddFolder" type="button" value="选择文件夹" />\
		                <input name="btnPasteFiles" id="btnPasteFiles" type="button" value="粘贴文件" />\
		            </div>\
		            <div class="content">\
		                <div name="pnlUploaderList" id="FilePostLister"></div>\
		            </div>\
		        </div>';
	    return acx;
	};

	this.setupTip = function ()
	{
	    this.uiToolBar.html('<a href="'+this.Config["ExePath"]+'">请安装控件</a>').css("cursor", "pointer")
	};
	this.setupCheck = function ()
	{
	    if (!this.browser.check())
	        this.setupTip();
	}

    //在外部调用。
	this.Load = function ()
	{
	    var html = this.GetHtml();
	    var ui = $(document.body).append(html);
	    this.InitUI(ui);
	    this.setupCheck();
	};

	this.LoadTo = function (oid)
	{
	    var html = this.GetHtml();
	    var ui = $("#" + oid).append(html);	    
	    this.InitUI(ui);
	    this.setupCheck();
	};

	this.InitUI = function (ui/*jquery object*/)
	{
	    var pnlUpload   = ui.find('div[name="pnlUpload"]');
	    var lstUpload   = ui.find('div[name="pnlUploaderList"]');
	    var tmpItem     = ui.find('div[name="tmpItem"]');
	    var droper      = ui.find('object[name="objDroper"]').get(0);
	    this.parter     = ui.find('object[name="parter"]').get(0);
	    if (this.npapi) this.parter = ui.find('embed[name="parter"]').get(0);
	    var btnAddFiles = ui.find('input[name="btnAddFiles"]');
	    var btnAddFd    = ui.find('input[name="btnAddFolder"]');
	    var btnPasteFs  = ui.find('input[name="btnPasteFiles"]');
	    var lnkClear    = ui.find('a[name="btnClear"]');
	    var spliter     = ui.find('div[name="spliter"]');
	    this.uiToolBar  = ui.find("div[name='toolbar']");

	    this.filesUI   = lstUpload;
	    this.tmpFile   = tmpItem;
        this.tmpLine   = spliter;
	    this.Droper    = droper;

	    btnAddFiles.click(function () { _this.browser.openFile(); });
	    btnAddFd.click(function () { _this.browser.openFolder(); });
	    btnPasteFs.click(function () { _this.browser.pasteFiles(); });
	    lnkClear.click(function () { _this.ClearComplete(); });

	    //droper
	    if (null != droper) droper.StateChanged = this.stateChanged;

	    this.browser.init(); //控件初始化
	};

	this.addFileLoc = function (f)
	{
	    //本地文件名称存在
	    //if (this.Exist(filePath)) return;
	    //此类型为过滤类型
	    //if (this.NeedFilter(filePath)) return;

	    var uper = new HttpUploader(f,this);
	    this.filesMap[f.id] = uper;
	    var ui = this.tmpFile.clone(true);
	    ui.css("display", "block");
	    this.filesUI.append(ui);//添加到文件列表

	    var line = this.tmpLine.clone();
	    line.css("display", "block");
	    this.filesUI.append(line);

	    var uiName      = ui.find("div[name='fileName']");
	    var uiSize      = ui.find("div[name='fileSize']")
	    var divProcess  = ui.find("div[name='process']");
	    var divMsg      = ui.find("div[name='msg']");
	    var btnCancel   = ui.find("span[name='btnCancel']");
	    var btnDel      = ui.find("span[name='btnDel']");
	    var divPercent  = ui.find("div[name='percent']");
	    var ui_eles = { msg: divMsg, percent: divPercent, process: divProcess, btn: {cancel:btnCancel,del:btnDel},panel:ui,spliter:line};

	    uper.ui = ui_eles;

	    uiName.text(f.name).attr("title", f.name);
	    uiSize.text(f.size);
	    divMsg.text("");
	    divPercent.text("0%");
	    btnCancel.click(function ()
	    {
	        var obj = $(this);
	        switch (obj.text())
	        {
	            case "暂停":
	            case "停止":
	                uper.stop();
	                break;
	            case "取消":
	                {
	                    uper.stop();
	                    uper.remove();
	                }
	                break;
	            case "续传":
	            case "重试":
	                uper.Post();
	                break;
	        }
	    });
	    btnDel.click(function () { uper.Delete(); });

	    uper.ready(); //准备
	};
}

var HttpUploaderState = {
	Ready: 0,
	Posting: 1,
	Stop: 2,
	Error: 3,
	GetNewID: 4,
	Complete: 5,
	WaitContinueUpload: 6,
	None: 7,
	Waiting: 8
};

//文件上传对象
function HttpUploader(f, mgr)
{
    var _this = this;
	//this.pMsg;
	//this.pProcess;
	//this.pPercent;
	//this.pButton
	//this.div
	//this.split
	//this.idLoc
	this.Manager = mgr; //上传管理器指针
	this.Config = mgr.Config;
	this.Fields = mgr.Fields;
	this.ActiveX = mgr.ActiveX;
	this.browser = mgr.browser;
	this.firefox = mgr.firefox;
	this.chrome = mgr.chrome;
	this.State = HttpUploaderState.None;
	this.fileLoc = f;
	this.ui = { msg: null, percent: null, process: null, btn: { cancel: null, del: null }, spliter: null, panel: null };

	this.ready = function ()
	{
	    this.ui.msg.text("正在上传队列中等待");
	};

	this.stop = function ()
	{
	    var inf = { name:"file_cancel",id: this.fileLoc.id };
	    this.browser.stop(inf);
	};

    //从上传列表中删除
	this.remove = function ()
	{
	    //删除信息层
	    this.ui.panel.remove();
	    //删除分隔线
	    this.ui.spliter.remove();
	};

	this.postError = function (json)
	{
	    this.ui.msg.text(HttpUploaderErrorCode[json.value]);
	    this.ui.btn.cancel.text("重试");
	};

	this.postComplete = function ()
	{
	    this.ui.btn.cancel.text("").hide();
	    this.ui.btn.del.hide();
	    this.ui.process.css("width", "100%");
	    this.ui.percent.text("100%");
	    this.ui.msg.text("上传完成");
	    this.State = HttpUploaderState.Complete;
	};

	this.postProcess = function (json)
	{
	    var msg = "已上传:" + json.len + " 速度:" + json.speed + " 剩余时间:" + json.time;
	    this.ui.msg.text(msg);
	    this.ui.percent.text(json.percent);
	    this.ui.process.css("width", json.percent);
	};
}