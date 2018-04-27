using System;
using System.Web;
using System.IO;
using Ionic.Zlib;

namespace HttpUploader2
{
    public partial class upload : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string v = Request.Form["UserName"];
            string compress = Request.Form["Compress"];//压缩模式。zip,gzip
			System.Diagnostics.Debug.WriteLine(v);

			if (Request.Files.Count > 0)
			{
				string folder = Server.MapPath("/upload");
				//自动创建上传文件夹
				if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

				HttpPostedFile file = Request.Files.Get(0);
				//获取纯文件名称:qq.exe
				string nameSvr = file.FileName;
                string ext = Path.GetExtension(nameSvr).ToLower();
				//只保存图片
				if (ext == ".asp"
					|| ext == ".aspx"
					|| ext == ".php")
				{
					return;
				}
				//合并路径
				string pathSvr = Path.Combine(folder, nameSvr);
                if (compress == "zip")
                {
                    this.saveZipData(ref file, pathSvr);
                }
                else if(compress == "gzip")
                {
                    this.saveGzipData(ref file, pathSvr);
                }//保存到服务器				
                else file.SaveAs(pathSvr);

				Response.Write(nameSvr);
			}
		}

        void saveZipData(ref HttpPostedFile file,string pathSvr)
        {
            //控件发送的文件数据是经过zlib压缩的，所以需要使用zlib解压
            using (ZlibStream zs = new ZlibStream(file.InputStream, CompressionMode.Decompress, false))
            {
                byte[] buffer = new byte[1024];
                int n = 0;
                MemoryStream ms = new MemoryStream();
                while ((n = zs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, n);
                }
                FileStream fs = new FileStream(pathSvr, FileMode.Create);
                ms.WriteTo(fs);
                fs.Close();
                ms.Close();
            }
        }

        void saveGzipData(ref HttpPostedFile file, string pathSvr)
        {
            //控件发送的文件数据是经过zlib压缩的，所以需要使用zlib解压
            using (GZipStream zs = new GZipStream(file.InputStream, CompressionMode.Decompress, false))
            {
                byte[] buffer = new byte[1024];
                int n = 0;
                MemoryStream ms = new MemoryStream();
                while ((n = zs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, n);
                }
                FileStream fs = new FileStream(pathSvr, FileMode.Create);
                ms.WriteTo(fs);
                fs.Close();
                ms.Close();
            }
        }
	}
}
