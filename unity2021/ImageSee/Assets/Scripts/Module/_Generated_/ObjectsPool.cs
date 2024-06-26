
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.88.0.  DO NOT EDIT!
//*************************************************************************************

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LibMVCS = XTC.FMP.LIB.MVCS;

namespace XTC.FMP.MOD.ImageSee.LIB.Unity
{
    public class ObjectsPoolMonoBehaviour : MonoBehaviour
    {

    }

    public class ObjectsPool
    {
        protected string uid_ { get; set; }
        protected LibMVCS.Logger logger_ { get; set; }

        protected MonoBehaviour mono_;

        /// <summary>
        /// 加载到内存的对象，键为加载路径
        /// </summary>
        protected Dictionary<string, UnityEngine.Object> objects = new Dictionary<string, UnityEngine.Object>();

        /// <summary>
        /// 加载到内存的文本，键为加载路径
        /// </summary>
        protected Dictionary<string, byte[]> texts = new Dictionary<string, byte[]>();

        /// <summary>
        /// 加载协程的列表，键为协程的独占编号
        /// </summary>
        private Dictionary<string, Coroutine> exclusiveCoroutines = new Dictionary<string, Coroutine>();

        public ObjectsPool(string _uid, LibMVCS.Logger _logger)
        {
            uid_ = string.Format("{0}.{1}", MyEntryBase.ModuleName, _uid);
            logger_ = _logger;
        }

        /// <summary>
        /// 准备
        /// </summary>
        public void Prepare()
        {
            if (null != mono_)
                return;
            var goMono = new GameObject(string.Format("[ObjectsPool_{0}]", uid_));
            mono_ = goMono.AddComponent<ObjectsPoolMonoBehaviour>();
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Dispose()
        {
            if (null == mono_)
                return;
            mono_.StopAllCoroutines();
            GameObject.Destroy(mono_.gameObject);
            mono_ = null;

            logger_.Trace("ObjectsPool:{0} ready to dispose {1} Objects", uid_, objects.Count);
            foreach (var obj in objects.Values)
            {
                UnityEngine.Object.Destroy(obj);
            }
            Resources.UnloadUnusedAssets();
            objects.Clear();
            logger_.Trace("ObjectsPool:{0} ready to dispose {1} Texts ", uid_, texts.Count);
            texts.Clear();
            // 执行一次垃圾回收
            GC.Collect();
        }

        /// <summary>
        /// 加载AduioClip
        /// </summary>
        /// <param name="_file">文件地址</param>
        /// <param name="_exclusiveNumber">独占编号，先终止正在运行的指定编号的协程, null为使用非独占模式</param>
        /// <param name="_onFinish">完成的回调</param>
        public void LoadAudioClip(string _file, string _exclusiveNumber, Action<AudioClip> _onFinish, Action _onError)
        {
            string file = _file.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            UnityEngine.Object obj;
            if (objects.TryGetValue(file, out obj))
            {
                _onFinish(obj as AudioClip);
                return;
            }

            Coroutine coroutine;
            if (null != _exclusiveNumber)
            {
                if (exclusiveCoroutines.TryGetValue(_exclusiveNumber, out coroutine))
                {
                    mono_.StopCoroutine(coroutine);
                    exclusiveCoroutines.Remove(_exclusiveNumber);
                }
            }
            coroutine = mono_.StartCoroutine(loadAudioClip(file, _exclusiveNumber, _onFinish, _onError));
            if (null != _exclusiveNumber)
            {
                exclusiveCoroutines[_exclusiveNumber] = coroutine;
            }
        }

        /// <summary>
        /// 加载Texture
        /// </summary>
        /// <param name="_file"></param>
        /// <param name="_onFinish"></param>
        public void LoadTexture(string _file, string _exclusiveNumber, Action<Texture2D> _onFinish, Action _onError)
        {
            string file = _file.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            UnityEngine.Object obj;
            if (objects.TryGetValue(file, out obj))
            {
                _onFinish(obj as Texture2D);
                return;
            }

            Coroutine coroutine;
            if (null != _exclusiveNumber)
            {
                if (exclusiveCoroutines.TryGetValue(_exclusiveNumber, out coroutine))
                {
                    mono_.StopCoroutine(coroutine);
                    exclusiveCoroutines.Remove(_exclusiveNumber);
                }
            }
            coroutine = mono_.StartCoroutine(loadTexture(file, _exclusiveNumber, _onFinish, _onError));
            if (null != _exclusiveNumber)
            {
                exclusiveCoroutines[_exclusiveNumber] = coroutine;
            }
        }

        /// <summary>
        /// 加载文本
        /// </summary>
        /// <param name="_file"></param>
        /// <param name="_onFinish"></param>
        public void LoadText(string _file, string _exclusiveNumber, Action<byte[]> _onFinish, Action _onError)
        {
            string file = _file.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            byte[] text;
            if (texts.TryGetValue(file, out text))
            {
                _onFinish(text);
                return;
            }

            Coroutine coroutine;
            if (null != _exclusiveNumber)
            {
                if (exclusiveCoroutines.TryGetValue(_exclusiveNumber, out coroutine))
                {
                    mono_.StopCoroutine(coroutine);
                    exclusiveCoroutines.Remove(_exclusiveNumber);
                }
            }
            coroutine = mono_.StartCoroutine(loadText(file, _exclusiveNumber, _onFinish, _onError));
            if (null != _exclusiveNumber)
            {
                exclusiveCoroutines[_exclusiveNumber] = coroutine;
            }
        }

        private IEnumerator loadAudioClip(string _file, string _exclusiveNumber, Action<AudioClip> _onFinish, Action _onError)
        {
            logger_.Trace("ready to load AduioClip from {0}", _file);
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(new Uri(_file), AudioType.MPEG))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    logger_.Trace("load {0} happen error", _file);
                    logger_.Error(uwr.error);
                    _onError();
                    yield break;
                }
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                objects[_file] = clip;
                logger_.Trace("load AduioClip:{0} success", _file);
                logger_.Trace("Currently, ObjectsPool:{0} has {1} Objects", uid_, objects.Count);
                if (null != _exclusiveNumber && exclusiveCoroutines.ContainsKey(_exclusiveNumber))
                {
                    exclusiveCoroutines.Remove(_exclusiveNumber);
                }
                _onFinish(clip);
            }
        }

        private IEnumerator loadTexture(string _file, string _exclusiveNumber, Action<Texture2D> _onFinish, Action _onError)
        {
            logger_.Trace("ready to load Texture from {0}", _file);
            using (var uwr = new UnityWebRequest(new Uri(_file)))
            {
                DownloadHandlerTexture handler = new DownloadHandlerTexture(true);
                uwr.downloadHandler = handler;
                yield return uwr.SendWebRequest();
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    logger_.Trace("load {0} happen error", _file);
                    logger_.Error(uwr.error);
                    _onError();
                    yield break;
                }
                Texture2D texture = handler.texture;
                objects[_file] = texture;
                logger_.Trace("load Texture:{0} success", _file);
                logger_.Trace("Currently, ObjectsPool:{0} has {1} Objects", uid_, objects.Count);
                if (null != _exclusiveNumber && exclusiveCoroutines.ContainsKey(_exclusiveNumber))
                {
                    exclusiveCoroutines.Remove(_exclusiveNumber);
                }
                _onFinish(texture);
            }
        }

        private IEnumerator loadText(string _file, string _exclusiveNumber, Action<byte[]> _onFinish, Action _onError)
        {
            logger_.Trace("ready to load Text from {0}", _file);
            using (var uwr = new UnityWebRequest(new Uri(_file)))
            {
                DownloadHandlerBuffer handler = new DownloadHandlerBuffer();
                uwr.downloadHandler = handler;
                yield return uwr.SendWebRequest();
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    logger_.Trace("load {0} happen error", _file);
                    logger_.Error(uwr.error);
                    _onError();
                    yield break;
                }
                byte[] bytes = handler.data;
                texts[_file] = bytes;
                logger_.Trace("load Text:{0} success", _file);
                logger_.Trace("Currently, ObjectsPool:{0} has {1} Texts", uid_, texts.Count);
                if (null != _exclusiveNumber && exclusiveCoroutines.ContainsKey(_exclusiveNumber))
                {
                    exclusiveCoroutines.Remove(_exclusiveNumber);
                }
                _onFinish(bytes);
            }
        }
    }
}
