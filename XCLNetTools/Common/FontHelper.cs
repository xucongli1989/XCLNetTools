using System.Collections.Generic;
using XCLNetTools.Entity;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 字体帮助类
    /// </summary>
    public static class FontHelper
    {
        /// <summary>
        /// 中文字体信息
        /// </summary>
        public static readonly List<FontInfoEntity> CNFontInfoList = new List<FontInfoEntity>() {
            new FontInfoEntity(){CNName="新细明体",ENName="PMingLiU" },
            new FontInfoEntity(){CNName="细明体",ENName="MingLiU" },
            new FontInfoEntity(){CNName="标楷体",ENName="DFKai-SB" },
            new FontInfoEntity(){CNName="黑体",ENName="SimHei" },
            new FontInfoEntity(){CNName="宋体",ENName="SimSun" },
            new FontInfoEntity(){CNName="新宋体",ENName="NSimSun" },
            new FontInfoEntity(){CNName="仿宋",ENName="FangSong" },
            new FontInfoEntity(){CNName="楷体",ENName="KaiTi" },
            new FontInfoEntity(){CNName="仿宋_GB2312",ENName="FangSong_GB2312" },
            new FontInfoEntity(){CNName="楷体_GB2312",ENName="KaiTi_GB2312" },
            new FontInfoEntity(){CNName="微软正黑体",ENName="Microsoft JhengHei" },
            new FontInfoEntity(){CNName="微软雅黑体",ENName="Microsoft YaHei" },
            new FontInfoEntity(){CNName="隶书",ENName="LiSu" },
            new FontInfoEntity(){CNName="幼圆",ENName="YouYuan" },
            new FontInfoEntity(){CNName="华文细黑",ENName="STXihei" },
            new FontInfoEntity(){CNName="华文楷体",ENName="STKaiti" },
            new FontInfoEntity(){CNName="华文宋体",ENName="STSong" },
            new FontInfoEntity(){CNName="华文中宋",ENName="STZhongsong" },
            new FontInfoEntity(){CNName="华文仿宋",ENName="STFangsong" },
            new FontInfoEntity(){CNName="方正舒体",ENName="FZShuTi" },
            new FontInfoEntity(){CNName="方正姚体",ENName="FZYaoti" },
            new FontInfoEntity(){CNName="华文彩云",ENName="STCaiyun" },
            new FontInfoEntity(){CNName="华文琥珀",ENName="STHupo" },
            new FontInfoEntity(){CNName="华文隶书",ENName="STLiti" },
            new FontInfoEntity(){CNName="华文行楷",ENName="STXingkai" },
            new FontInfoEntity(){CNName="华文新魏",ENName="STXinwei" }
        };
    }
}