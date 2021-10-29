namespace Server_Service
{
    partial class ServerInstallerCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServerServiceInstaller
            // 
            this.ServerServiceInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.ServerServiceInstaller.Password = null;
            this.ServerServiceInstaller.Username = null;
            // 
            // ServerInstaller
            // 
            this.ServerInstaller.Description = "A06 - Server Service";
            this.ServerInstaller.ServiceName = "_A06-Server";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServerServiceInstaller,
            this.ServerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServerServiceInstaller;
        private System.ServiceProcess.ServiceInstaller ServerInstaller;
    }
}