/*
	版权所有 2009-2015 荆门泽优软件有限公司
	保留所有权利
	官方网站：http://www.ncmem.com/
	产品首页：http://www.ncmem.com/webplug/http-uploader2/index.asp
	产品介绍：http://www.cnblogs.com/xproer/archive/2012/05/29/2523757.html
	开发文档-ASP：http://www.cnblogs.com/xproer/archive/2012/02/17/2355458.html
	开发文档-PHP：http://www.cnblogs.com/xproer/archive/2012/02/17/2355467.html
	开发文档-JSP：http://www.cnblogs.com/xproer/archive/2012/02/17/2355462.html
	开发文档-ASP.NET：http://www.cnblogs.com/xproer/archive/2012/02/17/2355469.html
	升级日志：http://www.cnblogs.com/xproer/archive/2012/02/17/2355449.html
	文档下载：http://yunpan.cn/lk/sVRrBEVQ7w5cw
	问题反馈：http://www.ncmem.com/bbs/showforum-19.aspx
	VC运行库：http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=29
	联系信箱：1085617561@qq.com
	联系QQ：1085617561
    更新记录：
	    2009-11-05 创建
        2015-08-01 优化
        2016-03-07 优化业务逻辑
*/

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
    this.Config = {
          "EncodeType"      : "GB2312"
		, "Company"         : "荆门泽优软件有限公司"
		, "Version"         : "2,5,53,40765"
		, "License"         : ""
		, "Debug"           : false//调试开关
		, "LogFile"         : "C:\\log.txt"//日志文件路径
		, "FileFilter"      : "*"//文件类型。所有类型：*。自定义类型：jpg,bmp,png,gif,rar,zip,7z,doc
	    //字节计算器：http://www.beesky.com/newsite/bit_byte.htm
	    //超过10MB建议选择HttpUploader6：http://www.ncmem.com/webplug/http-uploader6/index.asp
		, "FileSizeLimit"   : "10485760"//自定义允许上传的文件大小，以字节为单位。0表示不限制。字节计算工具：http://www.beesky.com/newsite/bit_byte.htm
		, "AllowMultiSelect": false//多选开关。true:开启多选。false:关闭多选
		, "InitDir"         : ""//初始化路径。示例：D:\\Soft
		, "Compress"		: ""//是否开启压缩。值：zip
		, "FileFieldName"	: "file"//文件字段名称
		, "Cookie"	        : ""//cookie
        //
		, "PostUrl"         : "http://localhost:4854/asp.net/upload.aspx"
        //x86
		, "ClsidPartition"  : "6F3EB4AF-FC9C-4570-A686-88B4B427C6FE"
		, "CabPath"         : "http://www.ncmem.com/download/HttpUploader2/HttpUploader.cab"
        //x64
        //x64
		, "ClsidPartition64": "3AFFCB6D-55ED-4ada-A1EC-D7D87BA29E51"
		, "CabPath64"       : "http://www.ncmem.com/download/HttpUploader2/HttpUploader64.cab"
        //Firefox
		, "XpiType"        : "application/npHttpUp2"
		, "XpiPath"         : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.xpi"
        //Chrome
		, "CrxName"         : "npHttpUp2"
		, "CrxType"         : "application/npHttpUp2"
		, "CrxPath"         : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.crx"
		, "ExePath"         : "http://www.ncmem.com/download/HttpUploader2/HttpUploader2.exe"
    };

    this.ActiveX = {
		"Partition": "Xproer.HttpPartition2"
        //64bit
		, "Partition64": "Xproer.HttpPartition2x64"
    };

    //附加参数
    this.Fields = {
        "uname": "test"
		, "upass": "test"
		, "uid": "0"
		, "fid": "0"
    };

    this.event = {
          postProcess: function (obj, speed, postedLength, percent, times) { }
	    , postComplete: function (obj) { }
        , postError: function (obj) { }
    };

    //http://www.ncmem.com/
    this.Domain = "http://" + document.location.host;
    this.parter = null;
    this.npapi = false;//
    this.fileItem = null;//jquery object
    this.fileCur = null;//当前文件上传项
    this.uiPanel = null;//显示UI界面的DIV，jquery object
    this.nat_loaded = false;

    this.postAll = function () { this.browser.post(); };
    this.part_files = function (ret)
    {
        if (ret.files == null) return;
        var f = ret.files[0];
        this.addFileLoc(f);
        //jQuery.extend(f, this.Fields);//附加字段
        //jQuery.extend(f, { name: "test" });
        this.browser.updateFile(f);//增加扩展信息
        var fn = function () { _this.browser.post(); };
        setTimeout(fn, 500);
    };
    this.post_complete = function (ret)
    {
        this.fileCur.postComplete();
        this.event.postComplete(this.fileCur);
        this.fileCur = null;
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
        var f = this.fileCur;
        f.postError(ret);
        this.event.postError(this.fileCur);
    };
    this.post_process = function (ret)
    {
        var f = this.fileCur;
        f.postProcess(ret);
    };
    this.queue_complete = function (ret)
    {
    };
    this.plugin_loaded = function (arg) { this.nat_loaded = true; }
    this.stateChanged = function (str)
    {
        var ret = JSON.parse(str);
        if (ret.name == "part_files") _this.part_files(ret);
        else if (ret.name == "post_complete") _this.post_complete(ret);
        else if (ret.name == "post_process") _this.post_process(ret);
        else if (ret.name == "post_error") _this.post_error(ret);
        else if (ret.name == "queue_complete") _this.queue_complete(ret);
        else if (ret.name == "plugin_loaded") _this.plugin_loaded(ret);
    };

    this.browser = {
          entID: "Uploader2Event"
        , check: function ()//检查插件是否已安装
        {
            try
            {
                return this.GetVersion() != "0";
            }
            catch (e) { return false; }
        }
        , checkFF: function ()//检查插件是否已安装
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
        , setupFF: function ()//安装插件
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
            var par = { name: "init", config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
            _this.parter.StateChanged = _this.stateChanged;
        }
        , initNat: function ()
        {
            this.exitEvent();
            document.addEventListener('Uploader2EventCallBack', function (evt)
            {
                _this.stateChanged(JSON.stringify(evt.detail));
            });
        }
        , post: function ()
        {
            var par = { name: 'queue_post', config: _this.Config, fields: _this.Fields };
            this.postMessage(par);
        }
        , stop: function (json/*id*/)
        {
            this.postMessage(json);
        }
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
        , postMessage: function (obj) 
        { 
            _this.parter.postMessage(JSON.stringify(obj));
        }
        , postMessageNat: function (json)
        {
            var evt = document.createEvent("CustomEvent");
            evt.initCustomEvent(this.entID, true, false, json);
            document.dispatchEvent(evt);
        }
	};

    //检查版本 Win32/Win64/Firefox/Chrome
    var browserName = navigator.userAgent.toLowerCase();
    _this.ie = browserName.indexOf("msie") > 0;
    //IE11检查
    _this.ie = _this.ie ? _this.ie : browserName.search(/(msie\s|trident.*rv:)([\w.]+)/) != -1;
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

    //文件上传面板。
    this.GetHtml = function ()
    {
        //加载拖拽控件
        var acx = "";
        //自动安装CAB
        //acx += '<div style="display:none">';
        //文件上传控件
        var plugin_html = '<object name="parter" classid="clsid:' + this.Config["ClsidPartition"] + '"';
	    plugin_html += ' codebase="' + this.Config["CabPath"] + '" width="1" height="1" ></object>';

	    if (this.firefox || this.chrome && !this.chrome45)
	    {
	        plugin_html = '<embed name="parter" type="' + this.Config["XpiType"] + '" pluginspage="' + this.Config["XpiPath"] + '" width="1" height="1"/>';
	    }
	    acx += plugin_html;
        //文件夹选择控件
        //acx += '<object name="parter" classid="clsid:' + _this.Config["ClsidPartition"] + '"';
        //acx += ' codebase="' + _this.Config["CabPath"] + '" width="1" height="1" ></object>';
        //acx += '</div>';
        //
        //		acx += '<div id="UploaderTemplate" class="sitem">';
        //		acx += '<div name="fileName" class="fileName">HttpUploader5-doc.rar</div>';
        //		acx += '<div name="fileSize" class="fileSize">(1.41MB)</div>';
        //		acx += '<div class="processbk"><div name="process" class="process"> </div></div>';
        //		acx += '<div name="btn" class="btn">删除</div>';
        //		acx += '<div name="btnCancel" class="btn hide">取消</div>';
        //		acx += '</div>';
        //上传列表项模板
        acx += '<div class="UploaderItem" name="fileItem" id="UploaderTemplate">\
		            <div class="UploaderItemLeft">\
		                <div class="FileInfo">\
		                    <div name="fileName" class="FileName top-space">HttpUploader程序开发.pdf</div>\
		                    <div name="fileSize" class="FileSize" child="1">100.23MB</div>\
		                </div>\
		                <div class="ProcessBorder top-space"><div name="process" class="Process"></div></div>\
		                <div name="msg" class="PostInf top-space">已上传:15.3MB 速度:20KB/S 剩余时间:10:02:00</div>\
		            </div>\
		            <div class="UploaderItemRight">\
		                <div class="BtnInfo"><span name="btnCancel" class="Btn">取消</span>&nbsp;<span name="btnDel" class="Btn">删除</span></div>\
		                <div name="percent" class="ProcessNum">35%</div>\
		            </div>\
		        </div>';
        return acx;
    };

    this.setupTip = function ()
    {
        var dom = $(document.body).append('<a href="' + this.Config["ExePath"] + '">请安装控件</a>');
        dom.css("cursor", "pointer");
    };
    this.setupCheck = function ()
    {
        if (!this.browser.check())
            this.setupTip();
    }

    this.Load = function ()
    {
        var html = this.GetHtml();
        var dom = $(document.body).append(html);
        this.fileItem = dom.find('div[name="fileItem"]');
        this.parter = dom.find('object[name="parter"]').get(0);
        if (this.npapi) this.parter = dom.find('embed[name="parter"]').get(0);
        this.browser.init(); //
        this.setupCheck();
    };

    //加截容器，上传面板，文件列表面板
    this.LoadTo = function (oid)
    {
        var html = this.GetHtml();
        var dom = $("#"+oid).html(html);
        this.fileItem = dom.find('div[name="fileItem"]');
        this.parter = dom.find('object[name="parter"]').get(0);
        if (this.npapi) this.parter = dom.find('embed[name="parter"]').get(0);
        this.browser.init(); //
        this.setupCheck();
    };

    //打开文件选择对话框
    this.OpenFile = function ()
    {
        _this.browser.openFile();
    };

    //粘贴文件
    this.PateFile = function ()
    {
        _this.browser.pasteFiles();
    };

    //oid,显示上传项的层ID
    this.postAuto = function (oid)
    {
        if (this.fileCur != null) return;
        this.uiPanel = $("#" + oid);
        this.OpenFile();
    };

    //上传文件
    this.postLoc = function (filePath, oid)
    {
        if (this.fileCur != null) return;
        this.uiPanel = $("#" + oid);
        this.browser.addFile(filePath);
    };
    this.addFileLoc = function (f)
    {
        var uper = new FileUploader(f, this);
        this.fileCur = uper;
        var ui = this.fileItem.clone(true);
        ui.css("display", "block");
        this.uiPanel.append(ui);

        var uiName      = ui.find("div[name='fileName']");
        var uiSize      = ui.find("div[name='fileSize']")
        var divProcess  = ui.find("div[name='process']");
        var divMsg      = ui.find("div[name='msg']");
        var btnCancel   = ui.find("span[name='btnCancel']");
        var btnDel      = ui.find("span[name='btnDel']");
        var divPercent  = ui.find("div[name='percent']");
        var ui_eles     = { msg: divMsg, percent: divPercent, process: divProcess, btn: { cancel: btnCancel, del: btnDel }, panel: ui};

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
                    uper.remove();
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
        btnCancel.hide();
        btnDel.click(function () { uper.remove(); _this.fileCur = null;});

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
	, MD5Working: 9
};

//文件上传对象
function FileUploader(f, mgr)
{
    var _this = this;
    //this.pMsg;
    //this.pProcess;
    //this.pPercent;
    //this.pButton
    //this.div
    //this.split
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

    //准备
    this.ready = function ()
    {
        this.ui.msg.text("正在上传队列中等待");
        this.State = HttpUploaderState.Ready;
    };

    this.stop = function ()
    {
        var inf = { name: "file_cancel", id: this.fileLoc.id };
        this.browser.stop(inf);
    };

    //从上传列表中删除
    this.remove = function ()
    {
        //删除信息层
        this.ui.panel.remove();
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