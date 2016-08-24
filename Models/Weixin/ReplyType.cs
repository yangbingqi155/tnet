namespace WeChatApp.Bll
{
    public class ReplyType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get { return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>"; }
        }



        public static string Message_Img
        {
            get
            {
                return @"<xml>
                            < ToUserName >< ![CDATA[{0}]] ></ ToUserName >
                            < FromUserName >< ![CDATA[{1}]] ></ FromUserName >
                            < CreateTime > {2} </ CreateTime >
                            < MsgType >< ![CDATA[image]] ></ MsgType >
                            < Image >
                            < MediaId >< ![CDATA[{3}]] ></ MediaId >
                            </ Image >
                            </ xml > ";
            }
        }



        /// <summary>
        /// 图文消息项
        /// </summary>
        public static string Message_News_Item
        {
            get
            {
                return @"<item>
                            <Title><![CDATA[{0}]]></Title> 
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                            </item>";
            }
        }


        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string Message_News_Main
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>
                            {4}
                            </Articles>
                            </xml> ";
            }
        }

        /// <summary>
        /// 视频消息
        /// </summary>
        public static string Message_Video
        {
            get
            {
                return @"<xml>
                        <ToUserName><![CDATA[{0}]]></ToUserName>
                        <FromUserName><![CDATA[{1}]]></FromUserName>
                        <CreateTime>{2}</CreateTime>
                        <MsgType><![CDATA[video]]></MsgType>
                        <Video>
                        <MediaId><![CDATA[{3}]]></MediaId>
                        <Title><![CDATA[{4}]]></Title>
                        <Description><![CDATA[{5}]]></Description>
                        </Video> 
                        </xml>";
            }
        }

        /// <summary>
        /// 音乐消息
        /// </summary>
        public static string Message_Music
        {
            get
            {
                return @"<xml>
                        <ToUserName><![CDATA[{0}]]></ToUserName>
                        <FromUserName><![CDATA[{1}]]></FromUserName>
                        <CreateTime>{2}</CreateTime>
                        <MsgType><![CDATA[music]]></MsgType>
                        <Music>
                        <Title><![CDATA[{3}]]></Title>
                        <Description><![CDATA[{4}]]></Description>
                        <MusicUrl><![CDATA[{5}]]></MusicUrl>
                        <HQMusicUrl><![CDATA[{6}]]></HQMusicUrl>
                        <ThumbMediaId><![CDATA[{7}]]></ThumbMediaId>
                        </Music>
                        </xml>";

            }
        }

        /// <summary>
        /// 语音消息
        /// </summary>
        public static string Message_Voice
        {
            get
            {

                return @"<xml>
                        <ToUserName><![CDATA[{0}]]></ToUserName>
                        <FromUserName><![CDATA[{1}]]></FromUserName>
                        <CreateTime>{2}</CreateTime>
                        <MsgType><![CDATA[voice]]></MsgType>
                        <Voice>
                        <MediaId><![CDATA[{3}]]></MediaId>
                        </Voice>
                        </xml>";
            }
        }
    }
}