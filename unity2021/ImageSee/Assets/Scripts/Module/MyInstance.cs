using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using System.IO;

namespace XTC.FMP.MOD.ImageSee.LIB.Unity
{
    /// <summary>
    /// 实例类
    /// </summary>
    public class MyInstance : MyInstanceBase
    {
        public class UiReference
        {
            public Image background;
            public RawImage image;
            public Transform tfToolBar;
            public RawImage pending;
            public Button btnZoomOut;
            public Button btnZoomIn;
        }

        private ContentReader contentReader_ = null;
        private UiReference uiReference_ = new UiReference();
        private float scale_ = 1f;
        private Vector2 originSizeDelta_;

        public MyInstance(string _uid, string _style, MyConfig _config, MyCatalog _catalog, LibMVCS.Logger _logger, Dictionary<string, LibMVCS.Any> _settings, MyEntryBase _entry, MonoBehaviour _mono, GameObject _rootAttachments)
            : base(_uid, _style, _config, _catalog, _logger, _settings, _entry, _mono, _rootAttachments)
        {
        }

        /// <summary>
        /// 当被创建时
        /// </summary>
        /// <remarks>
        /// 可用于加载主题目录的数据
        /// </remarks>
        public void HandleCreated()
        {
            uiReference_.background = rootUI.transform.Find("bg").GetComponent<Image>();
            uiReference_.image = rootUI.transform.Find("ScrollView/Viewport/Image").GetComponent<RawImage>();
            uiReference_.pending = rootUI.transform.Find("Pending").GetComponent<RawImage>();
            uiReference_.tfToolBar = rootUI.transform.Find("ToolBar");
            uiReference_.btnZoomOut = rootUI.transform.Find("ToolBar/btnZoomOut").GetComponent<Button>();
            uiReference_.btnZoomIn = rootUI.transform.Find("ToolBar/btnZoomIn").GetComponent<Button>();

            applyStyle();
            bindEvents();
        }

        /// <summary>
        /// 当被删除时
        /// </summary>
        public void HandleDeleted()
        {
        }

        /// <summary>
        /// 当被打开时
        /// </summary>
        /// <remarks>
        /// 可用于加载内容目录的数据
        /// </remarks>
        public void HandleOpened(string _source, string _uri)
        {
            contentReader_ = new ContentReader(contentObjectsPool);
            contentReader_.AssetRootPath = settings_["path.assets"].AsString();
            uiReference_.image.gameObject.SetActive(false);
            rootUI.gameObject.SetActive(true);
            uiReference_.pending.gameObject.SetActive(true);
            uiReference_.tfToolBar.gameObject.SetActive(false);
            loadImage(_source, _uri);
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            rootUI.gameObject.SetActive(false);
            contentReader_ = null;
        }

        private void loadImage(string _source, string _uri)
        {
            string contentUri = Path.GetDirectoryName(_uri);
            string file = Path.GetFileName(_uri);
            contentReader_.ContentUri = contentUri;
            contentReader_.LoadTexture(file, (_texture) =>
            {
                uiReference_.image.texture = _texture;
                uiReference_.image.SetNativeSize();
                fitImage();
                uiReference_.pending.gameObject.SetActive(false);
                uiReference_.image.gameObject.SetActive(true);
                uiReference_.tfToolBar.gameObject.SetActive(true);
                scale_ = 1.0f;
            }, () =>
            {

            });
        }

        private void fitImage()
        {
            var rtParent = uiReference_.image.transform.parent.GetComponent<RectTransform>();
            var rtImage = uiReference_.image.rectTransform;
            //图片和容器的宽度差值
            float differenceX = rtImage.rect.size.x - rtParent.rect.size.x;
            //图片和容器的高度差值
            float differenceY = rtImage.rect.size.y - rtParent.rect.size.y;
            float fitWidth = rtImage.rect.width;
            float fitHeight = rtImage.rect.height;
            if (differenceX > 0 || differenceY > 0)
            {
                if (differenceX > differenceY)
                {
                    fitWidth = rtParent.rect.size.x;
                    fitHeight = rtImage.rect.size.y / rtImage.rect.size.x * fitWidth;
                }
                else
                {
                    fitHeight = rtParent.rect.size.y;
                    fitWidth = rtImage.rect.size.x / rtImage.rect.size.y * fitHeight;
                }
            }
            originSizeDelta_ = new Vector2(fitWidth, fitHeight);
            rtImage.sizeDelta = originSizeDelta_;
        }

        private void applyStyle()
        {
            uiReference_.background.gameObject.SetActive(style_.background.visible);
            Color color;
            ColorUtility.TryParseHtmlString(style_.background.color, out color);
            uiReference_.background.color = color;

            loadTextureFromTheme(style_.pending.image, (_texture) =>
            {
                uiReference_.pending.texture = _texture;
                uiReference_.pending.SetNativeSize();
            }, () =>
            {

            });

            alignByAncor(uiReference_.tfToolBar, style_.toolBar.anchor);
        }

        private void bindEvents()
        {
            uiReference_.btnZoomIn.onClick.AddListener(() =>
            {
                scale_ += 0.2f;
                uiReference_.image.rectTransform.sizeDelta = originSizeDelta_ * scale_;

            });
            uiReference_.btnZoomOut.onClick.AddListener(() =>
            {
                scale_ -= 0.2f;
                if (scale_ < 1)
                    scale_ = 1;
                uiReference_.image.rectTransform.sizeDelta = originSizeDelta_ * scale_;
            });
        }
    }
}
