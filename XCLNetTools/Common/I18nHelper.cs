using I18Next.Net;
using I18Next.Net.Backends;
using I18Next.Net.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 多语言帮助类
    /// </summary>
    public static class I18nHelper
    {
        /// <summary>
        /// 常见语言列表
        /// </summary>
        public enum LanguageEnum
        {
            [Description("zh-CN")]
            简体中文,

            [Description("zh-TW")]
            繁体中文,

            [Description("en-US")]
            英语,

            [Description("fr-FR")]
            法语,

            [Description("de-DE")]
            德语,

            [Description("es-ES")]
            西班牙语,

            [Description("ja-JP")]
            日语,

            [Description("ko-KR")]
            韩语
        }

        /// <summary>
        /// 创建多语言实例
        /// </summary>
        public static I18NextNet CreateI18nInstance(List<Tuple<LanguageEnum, string, string>> transList)
        {
            var backend = new InMemoryBackend();

            transList.ForEach(k =>
            {
                backend.AddTranslation(k.Item1.ToString(), "translation", k.Item2, k.Item3);
            });

            var translator = new DefaultTranslator(backend);
            var i18Next = new I18NextNet(backend, translator);
            i18Next.SetFallbackLanguages("zh-CN");
            return i18Next;
        }
    }
}