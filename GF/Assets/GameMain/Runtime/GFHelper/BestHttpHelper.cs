using System;
using BestHTTP;
using GameFramework;
using GameFramework.WebRequest;
using UnityGameFramework.Runtime;

namespace GameMain.Runtime
{
    public class BestHttpHelper : WebRequestAgentHelperBase, IDisposable
    {
        private HTTPRequest httpRequest = null;
        private bool m_Disposed = false;

        private EventHandler<WebRequestAgentHelperCompleteEventArgs> m_WebRequestAgentHelperCompleteEventHandler = null;
        private EventHandler<WebRequestAgentHelperErrorEventArgs> m_WebRequestAgentHelperErrorEventHandler = null;


        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete
        {
            add { m_WebRequestAgentHelperCompleteEventHandler += value; }
            remove { m_WebRequestAgentHelperCompleteEventHandler -= value; }
        }

        /// <summary>
        /// Web 请求代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError
        {
            add { m_WebRequestAgentHelperErrorEventHandler += value; }
            remove { m_WebRequestAgentHelperErrorEventHandler -= value; }
        }

        public override void Request(string webRequestUri, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(userData.ToString());
            if (bytes.Length == 0)
            {
                httpRequest = new HTTPRequest(new Uri(webRequestUri), HTTPMethods.Get);
            }
            else
            {
                httpRequest = new HTTPRequest(new Uri(webRequestUri), HTTPMethods.Post);
                httpRequest.RawData = bytes;
                httpRequest.AddHeader("Content-Type", "application/json");
            }

            httpRequest.Send();
        }


        public override void Request(string webRequestUri, byte[] postData, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            httpRequest = new HTTPRequest(new Uri(webRequestUri), HTTPMethods.Post);
            httpRequest.RawData = postData;
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.Send();
        }

        public override void Reset()
        {
            if (httpRequest != null)
            {
                httpRequest.Dispose();
                httpRequest = null;
            }
        }

        public void Dispose()
        {
            if (m_Disposed)
            {
                return;
            }

            if (httpRequest != null)
            {
                httpRequest.Dispose();
                httpRequest = null;
            }

            m_Disposed = true;
        }

        private void Update()
        {
            if (httpRequest == null || httpRequest.State != HTTPRequestStates.Finished)
            {
                return;
            }

            bool isError = false;
            isError = httpRequest.State != HTTPRequestStates.Finished;
            if (isError)
            {
                WebRequestAgentHelperErrorEventArgs webRequestAgentHelperErrorEventArgs = WebRequestAgentHelperErrorEventArgs.Create(httpRequest.State.ToString());
                m_WebRequestAgentHelperErrorEventHandler(this, webRequestAgentHelperErrorEventArgs);
                ReferencePool.Release(webRequestAgentHelperErrorEventArgs);
            }
            else if (httpRequest.State == HTTPRequestStates.Finished)
            {
                WebRequestAgentHelperCompleteEventArgs webRequestAgentHelperCompleteEventArgs =
                    WebRequestAgentHelperCompleteEventArgs.Create(httpRequest.Response.Data);
                m_WebRequestAgentHelperCompleteEventHandler(this, webRequestAgentHelperCompleteEventArgs);
                ReferencePool.Release(webRequestAgentHelperCompleteEventArgs);
            }
        }
    }
}