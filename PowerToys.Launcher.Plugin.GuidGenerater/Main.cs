using Wox.Plugin;
using ManagedCommon;
using System.Windows.Input;

namespace PowerToys.Launcher.Plugin.GuidGenerater
{
    public class Main : IPlugin, IContextMenu
    {
        public string IconTheme { get; set; }

        public string Name => "guid";

        public string Description => "Guid生成器";

        private  PluginInitContext _ctx;

        public void Init(PluginInitContext context)
        {
            _ctx = context;
            _ctx.API.ThemeChanged += OnThemeChanged;
            UpdateIconTheme(_ctx.API.GetCurrentTheme());
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            var guid = Guid.NewGuid().ToString();
            var upper = guid.ToUpperInvariant();


            var lists = new List<string>()
            {
                guid,
                upper,
                guid.Replace("-",""),
                upper.Replace("-","")
            };
            foreach (var list in lists)
            {
                results.Add(new Result
                {
                    Title = list,
                    SubTitle = "选择或者按Ctrl-C复制",
                    IcoPath = "images\\logo." + IconTheme + ".png",
                    Action = _ => Components.ResultHelper.CopyToClipBoard(list),
                    ContextData = list,
                });
            }
            return results;
        }


        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconTheme(newTheme);
        }

        private void UpdateIconTheme(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconTheme = "light";
            }
            else
            {
                IconTheme = "dark";
            }
        }

        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (!(selectedResult?.ContextData is string data))
            {
                return new List<ContextMenuResult>(0);
            }
            return new List<ContextMenuResult>()
            {
                new ContextMenuResult
                {
                    AcceleratorKey = Key.C,
                    AcceleratorModifiers = ModifierKeys.Control,
                    FontFamily = "Segoe MDL2 Assets",
                    Glyph = "\xE8C8",                       // E8C8 => Symbol: Copy
                    Title = "复制到剪切板",
                    Action = _ =>Components.ResultHelper.CopyToClipBoard(data),
                },
            };
        }
    }
}