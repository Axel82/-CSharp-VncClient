using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VncSharp;

namespace WPF_VNC
{
    public delegate void NotifyConnectionOpened();
    public delegate void NotifyConnectionLost();
    public delegate void NotifyConnectionError();

    public partial class MafVnc : UserControl
    {
        private class Parameters
        {
            public string ip = "";
            public int port = 5900;
            public string password="";
        }

        #region Fields
        bool manualDisconnection = false;
        Parameters vncParameters;
        int reconnectionCounter;
        #endregion

        #region Connection events
        /// <summary>
        /// Event to know, the connection is opened
        /// </summary>
        public event NotifyConnectionOpened vncConnectionOpened;
        /// <summary>
        /// Event to know the connection is lost
        /// </summary>
        public event NotifyConnectionLost vncConnectionLost;
        /// <summary>
        /// Event to know the connection is in error
        /// </summary>
        public event NotifyConnectionError vncConnectionError;
        #endregion

        #region Constructor
        /// <summary>
        /// MAF VNC contains all methods & Events pour 'Start' & 'Stop' a VNC connection.
        /// </summary>
        public MafVnc()
        {
            InitializeComponent();

            // VNC parameters
            reconnectionCounter = 0;
            vncParameters = new Parameters();

            // Remote desktop events
            remoteDesktop.ConnectComplete += new ConnectCompleteHandler(ConnectComplete);
            remoteDesktop.ConnectionLost += new EventHandler(ConnectionLost);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Start the connect with VNC server.
        /// </summary>
        /// <param name="vncIp">IP V4</param>
        /// <param name="vncPort">Port number</param>
        /// <param name="vncPassword">Connection password. Only if necessary</param>
        public void Start(string vncIp, int vncPort, string vncPassword="")
        {
            // Set VNC parameters
            vncParameters.ip = vncIp;
            vncParameters.port = vncPort;
            vncParameters.password = vncPassword;

            // Start VNC connection
            try
            {
                // VNC port
                remoteDesktop.VncPort = vncParameters.port;

                // VNC password
                if (vncParameters.password != "")
                    remoteDesktop.GetPassword = new AuthenticateDelegate(() => vncParameters.password);

                // VNC IP connect
                remoteDesktop.Connect(vncParameters.ip);

                // Opener event
                onVncConnectionOpened();
            }
            catch (Exception exc)
            {
                // Connection error event
                onVncConnectionError();

                // Error display
                Console.WriteLine(string.Format("Error in 'Start'\r\n==> {0}", exc.Message));
            }
            finally { }
        }

        /// <summary>
        /// Stop the connection with VNC server
        /// </summary>
        public void Stop()
        {
            if(remoteDesktop.IsConnected)
            {
                manualDisconnection = true;
                remoteDesktop.Disconnect();
            }
        }
        #endregion

        #region VNC Protected methods
        protected void ConnectComplete(object sender, ConnectEventArgs e)
        {
            // Update Form to match geometry of remote desktop
            ClientSize = new Size(e.DesktopWidth,
                                  e.DesktopHeight);

            remoteDesktop.Height = ClientSize.Height;
            remoteDesktop.Width = ClientSize.Width;
        }

        protected void ConnectionLost(object sender, EventArgs e)
        {
            if(!manualDisconnection)
            {
                // User information : connection lost
                onVncConnectionLost();

                // Try to reconnect
                if (reconnectionCounter == 0 && remoteDesktop.IsConnected == false)
                    Reconnection();
            }
        }


        #endregion

        #region Private Tasks methods
        private void Reconnection()
        {
            // Method 1
            //Task myReconnectionTask = Task.Run(() => ReconnectionTask());



            // Method 2
            try
            {
                // Increase reconnectionCounter because we retry
                reconnectionCounter++;

                // Wait 30 sec
                var t = Task.Run(async delegate
                {
                    await Task.Delay(30000);
                });
                t.Wait();

                // VNC IP connect
                if (remoteDesktop.IsConnected)
                    remoteDesktop.Disconnect();
                remoteDesktop.Connect(vncParameters.ip);

                // Reset reconnectionCounter because we are connected
                reconnectionCounter = 0;
            }
            catch (Exception exc)
            {
                // Error display
                Console.WriteLine(string.Format("Error in 'Reconnection' it:{0}\r\n==> {1}", reconnectionCounter, exc.Message));
            }
            finally
            {
                // Try again or not
                if(reconnectionCounter > 0)
                    Reconnection();
            }

        }
        private void ReconnectionTask()
        {
            try
            {
                // Increase reconnectionCounter because we retry
                reconnectionCounter++;

                // Wait 30 sec
                var t = Task.Run(async delegate
                {
                    await Task.Delay(30000);
                });
                t.Wait();

                // VNC IP connect
                remoteDesktop.Connect(vncParameters.ip);

                // Reset reconnectionCounter because we are connected
                reconnectionCounter = 0;
            }
            catch (Exception exc)
            {
                // Error display
                Console.WriteLine(string.Format("Error in 'ReconnectionTask' it:{0}\r\n==> {1}", reconnectionCounter, exc.Message));
            }
            finally 
            {
                // Try again
                Reconnection();
            }
        }
        #endregion

        #region Events
        protected virtual void onVncConnectionOpened()
        {
            //if vncConnectionOpened is not null then call delegate
            vncConnectionOpened?.Invoke();
        }

        protected virtual void onVncConnectionLost()
        {
            //if vncConnectionLost is not null then call delegate
            vncConnectionLost?.Invoke();
        }

        protected virtual void onVncConnectionError()
        {
            //if vncConnectionError is not null then call delegate
            vncConnectionError?.Invoke();
        }
        #endregion
    }
}
