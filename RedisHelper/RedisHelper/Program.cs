using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379/0","RedisHelper.Demo");
            //订阅 Channel1 频道
            redisHelper.Subscribe("Channel1", new Action<RedisChannel, RedisValue>((channel, message) =>
            {
                Console.WriteLine("Channel1" + " 订阅收到消息：" + message);
            }));
            for (int i = 0; i < 10; i++)
            {
                redisHelper.Publish("Channel1", (RedisValue)("msg" + i));//向频道 Channel1 发送信息
                if (i == 2)
                {
                    redisHelper.Unsubscribe("Channel1");//取消订阅//因为当 i == 2 的时候取消订阅，所以收到的订阅消息只有3条
                }
            }

            Model model = new Model();
            model.Id = 1;
            model.Name = "用户1";
            model.Gender = (int)Gender.Male;
            model.BirthDay = DateTime.Now.AddYears(-25);

            redisHelper.HashSet<Model>("User","user1", model);

            model.Id = 2;model.Name = "用户2";model.Gender = (int)Gender.Male;model.BirthDay = DateTime.Now.AddYears(-19);

            redisHelper.HashSet<Model>("User", "user2", model);
            model = redisHelper.HashGet<Model>("User", "user1");

            List<Model> models = new List<Model>();
            models = redisHelper.HashValues<Model>("User").ToList();

            models.Add(new Model() { Id=3, Name="用户3", Gender= (int)Gender.Male , BirthDay= DateTime.Now.AddYears(-19) });
            //redisHelper.ListLeftPush<List<Model>>("7:config:user",models);//1.此方法导致二维数组
            redisHelper.ListLeftPush<Model>("7:config:user", models);

            models = redisHelper.ListRange<Model>("7:config:user").ToList();//1.进而导致此处反序列化时List<T>转换为T失败

            Console.ReadKey();
        }
    }

    [Serializable]
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Gender { get; set; }
        public DateTime? BirthDay {get;set;}
    }
    public enum Gender
    {
        Male=0,
        FaMale=1
    }
}
