namespace Celiameter
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
      this.lvThumbs = new System.Windows.Forms.ListView();
      this.txtPointerCoords = new System.Windows.Forms.ToolStripStatusLabel();
      this.statusStrip2 = new System.Windows.Forms.StatusStrip();
      this.txtDebugMsg = new System.Windows.Forms.ToolStripStatusLabel();
      this.btnSURF = new System.Windows.Forms.ToolStripButton();
      this.toolStrip2 = new System.Windows.Forms.ToolStrip();
      this.btnCanny = new System.Windows.Forms.ToolStripButton();
      this.splitRight = new System.Windows.Forms.SplitContainer();
      this.pbMain = new Emgu.CV.UI.ImageBox();
      this.thumbToolStrip = new System.Windows.Forms.ToolStrip();
      this.pbcFR = new System.Windows.Forms.ToolStripButton();
      this.pbcStop = new System.Windows.Forms.ToolStripButton();
      this.pbcPlay = new System.Windows.Forms.ToolStripButton();
      this.pbcPlayPause = new System.Windows.Forms.ToolStripButton();
      this.pbcFF = new System.Windows.Forms.ToolStripButton();
      this.pbcLabel = new System.Windows.Forms.ToolStripLabel();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tbCurImage = new System.Windows.Forms.ToolStripTextBox();
      this.btnCacheAllFrames = new System.Windows.Forms.ToolStripButton();
      this.pbOutView = new Emgu.CV.UI.ImageBox();
      this.splitContorl = new System.Windows.Forms.SplitContainer();
      this.chkOverlayDiff = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.cbSessioFPS = new System.Windows.Forms.ComboBox();
      this.splitLeft = new System.Windows.Forms.SplitContainer();
      this.pbZoom = new Emgu.CV.UI.ImageBox();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
      this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.cbSeperator = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
      this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.statusStrip2.SuspendLayout();
      this.toolStrip2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
      this.splitRight.Panel1.SuspendLayout();
      this.splitRight.Panel2.SuspendLayout();
      this.splitRight.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
      this.thumbToolStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbOutView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContorl)).BeginInit();
      this.splitContorl.Panel1.SuspendLayout();
      this.splitContorl.Panel2.SuspendLayout();
      this.splitContorl.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).BeginInit();
      this.splitLeft.Panel1.SuspendLayout();
      this.splitLeft.Panel2.SuspendLayout();
      this.splitLeft.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // lvThumbs
      // 
      this.lvThumbs.Activation = System.Windows.Forms.ItemActivation.TwoClick;
      this.lvThumbs.Alignment = System.Windows.Forms.ListViewAlignment.Left;
      this.lvThumbs.AutoArrange = false;
      this.lvThumbs.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lvThumbs.GridLines = true;
      this.lvThumbs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lvThumbs.HideSelection = false;
      this.lvThumbs.Location = new System.Drawing.Point(0, 0);
      this.lvThumbs.MultiSelect = false;
      this.lvThumbs.Name = "lvThumbs";
      this.lvThumbs.OwnerDraw = true;
      this.lvThumbs.ShowGroups = false;
      this.lvThumbs.Size = new System.Drawing.Size(1247, 281);
      this.lvThumbs.TabIndex = 0;
      this.lvThumbs.UseCompatibleStateImageBehavior = false;
      this.lvThumbs.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvThumbs_DrawItem);
      this.lvThumbs.ItemActivate += new System.EventHandler(this.lvThumbs_ItemActivate);
      this.lvThumbs.SelectedIndexChanged += new System.EventHandler(this.lvThumbs_SelectedIndexChanged);
      // 
      // txtPointerCoords
      // 
      this.txtPointerCoords.Name = "txtPointerCoords";
      this.txtPointerCoords.Size = new System.Drawing.Size(54, 41);
      this.txtPointerCoords.Text = "///";
      // 
      // statusStrip2
      // 
      this.statusStrip2.ImageScalingSize = new System.Drawing.Size(40, 40);
      this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtPointerCoords,
            this.txtDebugMsg});
      this.statusStrip2.Location = new System.Drawing.Point(0, 602);
      this.statusStrip2.Name = "statusStrip2";
      this.statusStrip2.Size = new System.Drawing.Size(1247, 46);
      this.statusStrip2.TabIndex = 1;
      this.statusStrip2.Text = "statusStrip2";
      // 
      // txtDebugMsg
      // 
      this.txtDebugMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.txtDebugMsg.Name = "txtDebugMsg";
      this.txtDebugMsg.Size = new System.Drawing.Size(54, 41);
      this.txtDebugMsg.Text = "///";
      this.txtDebugMsg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.txtDebugMsg.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
      // 
      // btnSURF
      // 
      this.btnSURF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btnSURF.Image = ((System.Drawing.Image)(resources.GetObject("btnSURF.Image")));
      this.btnSURF.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnSURF.Name = "btnSURF";
      this.btnSURF.Size = new System.Drawing.Size(92, 45);
      this.btnSURF.Text = "SURF";
      this.btnSURF.Click += new System.EventHandler(this.btnSURF_Click);
      // 
      // toolStrip2
      // 
      this.toolStrip2.ImageScalingSize = new System.Drawing.Size(40, 40);
      this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCanny,
            this.btnSURF});
      this.toolStrip2.Location = new System.Drawing.Point(0, 0);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new System.Drawing.Size(1247, 48);
      this.toolStrip2.TabIndex = 3;
      this.toolStrip2.Text = "toolStrip2";
      // 
      // btnCanny
      // 
      this.btnCanny.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btnCanny.Image = ((System.Drawing.Image)(resources.GetObject("btnCanny.Image")));
      this.btnCanny.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnCanny.Name = "btnCanny";
      this.btnCanny.Size = new System.Drawing.Size(121, 45);
      this.btnCanny.Text = "CANNY";
      this.btnCanny.Click += new System.EventHandler(this.btnCanny_Click);
      // 
      // splitRight
      // 
      this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitRight.Location = new System.Drawing.Point(0, 0);
      this.splitRight.Name = "splitRight";
      this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitRight.Panel1
      // 
      this.splitRight.Panel1.Controls.Add(this.toolStrip2);
      this.splitRight.Panel1.Controls.Add(this.pbMain);
      this.splitRight.Panel1.Controls.Add(this.statusStrip2);
      // 
      // splitRight.Panel2
      // 
      this.splitRight.Panel2.Controls.Add(this.thumbToolStrip);
      this.splitRight.Panel2.Controls.Add(this.lvThumbs);
      this.splitRight.Size = new System.Drawing.Size(1247, 933);
      this.splitRight.SplitterDistance = 648;
      this.splitRight.TabIndex = 1;
      // 
      // pbMain
      // 
      this.pbMain.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
      this.pbMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pbMain.Location = new System.Drawing.Point(0, 0);
      this.pbMain.Name = "pbMain";
      this.pbMain.Size = new System.Drawing.Size(1247, 602);
      this.pbMain.TabIndex = 2;
      this.pbMain.TabStop = false;
      this.pbMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbMain_MouseClick);
      this.pbMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbMain_MouseDoubleClick);
      this.pbMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMain_MouseDown);
      this.pbMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMain_MouseMove);
      this.pbMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbMain_MouseUp);
      // 
      // thumbToolStrip
      // 
      this.thumbToolStrip.ImageScalingSize = new System.Drawing.Size(38, 38);
      this.thumbToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbcFR,
            this.pbcStop,
            this.pbcPlay,
            this.pbcPlayPause,
            this.pbcFF,
            this.pbcLabel,
            this.toolStripSeparator1,
            this.tbCurImage,
            this.btnCacheAllFrames});
      this.thumbToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
      this.thumbToolStrip.Location = new System.Drawing.Point(0, 0);
      this.thumbToolStrip.Name = "thumbToolStrip";
      this.thumbToolStrip.ShowItemToolTips = false;
      this.thumbToolStrip.Size = new System.Drawing.Size(1247, 48);
      this.thumbToolStrip.Stretch = true;
      this.thumbToolStrip.TabIndex = 1;
      this.thumbToolStrip.Text = "Thumbs Toolstrip";
      // 
      // pbcFR
      // 
      this.pbcFR.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.pbcFR.Image = ((System.Drawing.Image)(resources.GetObject("pbcFR.Image")));
      this.pbcFR.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.pbcFR.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pbcFR.Name = "pbcFR";
      this.pbcFR.Size = new System.Drawing.Size(42, 42);
      this.pbcFR.Text = "toolStripButton3";
      this.pbcFR.Click += new System.EventHandler(this.thumbStrip_playControlClicked);
      // 
      // pbcStop
      // 
      this.pbcStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.pbcStop.Image = ((System.Drawing.Image)(resources.GetObject("pbcStop.Image")));
      this.pbcStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.pbcStop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pbcStop.Name = "pbcStop";
      this.pbcStop.Size = new System.Drawing.Size(42, 42);
      this.pbcStop.Text = "toolStripButton2";
      this.pbcStop.Click += new System.EventHandler(this.thumbStrip_playControlClicked);
      // 
      // pbcPlay
      // 
      this.pbcPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.pbcPlay.Image = ((System.Drawing.Image)(resources.GetObject("pbcPlay.Image")));
      this.pbcPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.pbcPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pbcPlay.Name = "pbcPlay";
      this.pbcPlay.Size = new System.Drawing.Size(38, 42);
      this.pbcPlay.Text = "toolStripButton1";
      this.pbcPlay.Click += new System.EventHandler(this.thumbStrip_playControlClicked);
      // 
      // pbcPlayPause
      // 
      this.pbcPlayPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.pbcPlayPause.Image = ((System.Drawing.Image)(resources.GetObject("pbcPlayPause.Image")));
      this.pbcPlayPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.pbcPlayPause.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pbcPlayPause.Name = "pbcPlayPause";
      this.pbcPlayPause.Size = new System.Drawing.Size(42, 42);
      this.pbcPlayPause.Text = "toolStripButton5";
      this.pbcPlayPause.Click += new System.EventHandler(this.thumbStrip_playControlClicked);
      // 
      // pbcFF
      // 
      this.pbcFF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.pbcFF.Image = ((System.Drawing.Image)(resources.GetObject("pbcFF.Image")));
      this.pbcFF.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.pbcFF.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pbcFF.Name = "pbcFF";
      this.pbcFF.Size = new System.Drawing.Size(42, 42);
      this.pbcFF.Text = "toolStripButton4";
      this.pbcFF.Click += new System.EventHandler(this.thumbStrip_playControlClicked);
      // 
      // pbcLabel
      // 
      this.pbcLabel.AutoSize = false;
      this.pbcLabel.AutoToolTip = true;
      this.pbcLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.pbcLabel.Name = "pbcLabel";
      this.pbcLabel.Size = new System.Drawing.Size(158, 41);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
      // 
      // tbCurImage
      // 
      this.tbCurImage.AutoSize = false;
      this.tbCurImage.Name = "tbCurImage";
      this.tbCurImage.ReadOnly = true;
      this.tbCurImage.Size = new System.Drawing.Size(100, 47);
      // 
      // btnCacheAllFrames
      // 
      this.btnCacheAllFrames.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btnCacheAllFrames.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
      this.btnCacheAllFrames.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheAllFrames.Image")));
      this.btnCacheAllFrames.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnCacheAllFrames.Name = "btnCacheAllFrames";
      this.btnCacheAllFrames.Size = new System.Drawing.Size(156, 45);
      this.btnCacheAllFrames.Text = "Cache All";
      this.btnCacheAllFrames.ToolTipText = "Cache All images in memory.\\r\\n warning - may be time consuming";
      this.btnCacheAllFrames.Click += new System.EventHandler(this.btnCacheAllFrames_Click);
      // 
      // pbOutView
      // 
      this.pbOutView.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
      this.pbOutView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pbOutView.Location = new System.Drawing.Point(0, 0);
      this.pbOutView.Name = "pbOutView";
      this.pbOutView.Size = new System.Drawing.Size(624, 269);
      this.pbOutView.TabIndex = 3;
      this.pbOutView.TabStop = false;
      // 
      // splitContorl
      // 
      this.splitContorl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContorl.Location = new System.Drawing.Point(0, 0);
      this.splitContorl.Name = "splitContorl";
      this.splitContorl.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContorl.Panel1
      // 
      this.splitContorl.Panel1.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
      this.splitContorl.Panel1.AutoScroll = true;
      this.splitContorl.Panel1.Controls.Add(this.chkOverlayDiff);
      this.splitContorl.Panel1.Controls.Add(this.label1);
      this.splitContorl.Panel1.Controls.Add(this.cbSessioFPS);
      // 
      // splitContorl.Panel2
      // 
      this.splitContorl.Panel2.Controls.Add(this.pbOutView);
      this.splitContorl.Size = new System.Drawing.Size(624, 548);
      this.splitContorl.SplitterDistance = 275;
      this.splitContorl.TabIndex = 0;
      // 
      // chkOverlayDiff
      // 
      this.chkOverlayDiff.AutoSize = true;
      this.chkOverlayDiff.Location = new System.Drawing.Point(22, 61);
      this.chkOverlayDiff.Name = "chkOverlayDiff";
      this.chkOverlayDiff.Size = new System.Drawing.Size(201, 36);
      this.chkOverlayDiff.TabIndex = 2;
      this.chkOverlayDiff.Text = "Overlay Diff";
      this.chkOverlayDiff.UseVisualStyleBackColor = true;
      this.chkOverlayDiff.CheckedChanged += new System.EventHandler(this.chkOverlayDiff_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(70, 32);
      this.label1.TabIndex = 1;
      this.label1.Text = "FPS";
      // 
      // cbSessioFPS
      // 
      this.cbSessioFPS.FormatString = "##.#";
      this.cbSessioFPS.FormattingEnabled = true;
      this.cbSessioFPS.Items.AddRange(new object[] {
            "24",
            "25",
            "29",
            "30",
            "60",
            "120",
            "240",
            "360",
            "500",
            "600",
            "1200"});
      this.cbSessioFPS.Location = new System.Drawing.Point(92, 9);
      this.cbSessioFPS.Name = "cbSessioFPS";
      this.cbSessioFPS.Size = new System.Drawing.Size(159, 39);
      this.cbSessioFPS.TabIndex = 0;
      this.cbSessioFPS.Text = "60";
      this.cbSessioFPS.TextChanged += new System.EventHandler(this.cbSessioFPS_TextChanged);
      // 
      // splitLeft
      // 
      this.splitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitLeft.Location = new System.Drawing.Point(0, 0);
      this.splitLeft.Name = "splitLeft";
      this.splitLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitLeft.Panel1
      // 
      this.splitLeft.Panel1.Controls.Add(this.splitContorl);
      // 
      // splitLeft.Panel2
      // 
      this.splitLeft.Panel2.Controls.Add(this.pbZoom);
      this.splitLeft.Size = new System.Drawing.Size(624, 933);
      this.splitLeft.SplitterDistance = 548;
      this.splitLeft.TabIndex = 0;
      // 
      // pbZoom
      // 
      this.pbZoom.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
      this.pbZoom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pbZoom.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
      this.pbZoom.Location = new System.Drawing.Point(0, 0);
      this.pbZoom.Margin = new System.Windows.Forms.Padding(0);
      this.pbZoom.Name = "pbZoom";
      this.pbZoom.Size = new System.Drawing.Size(624, 381);
      this.pbZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.pbZoom.TabIndex = 2;
      this.pbZoom.TabStop = false;
      // 
      // splitMain
      // 
      this.splitMain.Cursor = System.Windows.Forms.Cursors.Default;
      this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitMain.Location = new System.Drawing.Point(0, 48);
      this.splitMain.Name = "splitMain";
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.splitLeft);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.splitRight);
      this.splitMain.Size = new System.Drawing.Size(1875, 933);
      this.splitMain.SplitterDistance = 624;
      this.splitMain.TabIndex = 5;
      // 
      // helpToolStripButton
      // 
      this.helpToolStripButton.AutoSize = false;
      this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
      this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.helpToolStripButton.Name = "helpToolStripButton";
      this.helpToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.helpToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.helpToolStripButton.Text = "He&lp";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.AutoSize = false;
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.toolStripSeparator2.Size = new System.Drawing.Size(40, 40);
      // 
      // pasteToolStripButton
      // 
      this.pasteToolStripButton.AutoSize = false;
      this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
      this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pasteToolStripButton.Name = "pasteToolStripButton";
      this.pasteToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.pasteToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.pasteToolStripButton.Text = "&Paste";
      // 
      // copyToolStripButton
      // 
      this.copyToolStripButton.AutoSize = false;
      this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
      this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.copyToolStripButton.Name = "copyToolStripButton";
      this.copyToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.copyToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.copyToolStripButton.Text = "&Copy";
      // 
      // cutToolStripButton
      // 
      this.cutToolStripButton.AutoSize = false;
      this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
      this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.cutToolStripButton.Name = "cutToolStripButton";
      this.cutToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.cutToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.cutToolStripButton.Text = "C&ut";
      // 
      // toolStripSeparator
      // 
      this.toolStripSeparator.AutoSize = false;
      this.toolStripSeparator.Name = "toolStripSeparator";
      this.toolStripSeparator.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.toolStripSeparator.Size = new System.Drawing.Size(40, 40);
      // 
      // printToolStripButton
      // 
      this.printToolStripButton.AutoSize = false;
      this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
      this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.printToolStripButton.Name = "printToolStripButton";
      this.printToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.printToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.printToolStripButton.Text = "&Print";
      // 
      // saveToolStripButton
      // 
      this.saveToolStripButton.AutoSize = false;
      this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
      this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveToolStripButton.Name = "saveToolStripButton";
      this.saveToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.saveToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.saveToolStripButton.Text = ".";
      this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
      // 
      // openToolStripButton
      // 
      this.openToolStripButton.AutoSize = false;
      this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
      this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openToolStripButton.Name = "openToolStripButton";
      this.openToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.openToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.openToolStripButton.Text = "&Open";
      // 
      // newToolStripButton
      // 
      this.newToolStripButton.AutoSize = false;
      this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
      this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newToolStripButton.Name = "newToolStripButton";
      this.newToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.newToolStripButton.Size = new System.Drawing.Size(40, 40);
      this.newToolStripButton.Text = "&New";
      this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.AllowMerge = false;
      this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator2,
            this.helpToolStripButton,
            this.cbSeperator});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1875, 48);
      this.toolStrip1.Stretch = true;
      this.toolStrip1.TabIndex = 4;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // cbSeperator
      // 
      this.cbSeperator.Name = "cbSeperator";
      this.cbSeperator.Size = new System.Drawing.Size(6, 48);
      // 
      // toolStripProgressBar1
      // 
      this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripProgressBar1.Name = "toolStripProgressBar1";
      this.toolStripProgressBar1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.toolStripProgressBar1.Size = new System.Drawing.Size(400, 42);
      // 
      // toolStripStatusLabel3
      // 
      this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
      this.toolStripStatusLabel3.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.toolStripStatusLabel3.Size = new System.Drawing.Size(864, 43);
      this.toolStripStatusLabel3.Spring = true;
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(297, 43);
      this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(297, 43);
      this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
      // 
      // statusStrip1
      // 
      this.statusStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripProgressBar1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 981);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(1875, 48);
      this.statusStrip1.TabIndex = 3;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // frmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1875, 1029);
      this.Controls.Add(this.splitMain);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.statusStrip1);
      this.Name = "frmMain";
      this.Text = "Form1";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
      this.Load += new System.EventHandler(this.frmMain_Load);
      this.Shown += new System.EventHandler(this.frmMain_Shown);
      this.statusStrip2.ResumeLayout(false);
      this.statusStrip2.PerformLayout();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.splitRight.Panel1.ResumeLayout(false);
      this.splitRight.Panel1.PerformLayout();
      this.splitRight.Panel2.ResumeLayout(false);
      this.splitRight.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
      this.splitRight.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
      this.thumbToolStrip.ResumeLayout(false);
      this.thumbToolStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbOutView)).EndInit();
      this.splitContorl.Panel1.ResumeLayout(false);
      this.splitContorl.Panel1.PerformLayout();
      this.splitContorl.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContorl)).EndInit();
      this.splitContorl.ResumeLayout(false);
      this.splitLeft.Panel1.ResumeLayout(false);
      this.splitLeft.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).EndInit();
      this.splitLeft.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).EndInit();
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
      this.splitMain.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvThumbs;
        private System.Windows.Forms.ToolStripStatusLabel txtPointerCoords;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel txtDebugMsg;
        private System.Windows.Forms.ToolStripButton btnSURF;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.SplitContainer splitRight;
        private Emgu.CV.UI.ImageBox pbMain;
        private Emgu.CV.UI.ImageBox pbOutView;
        private System.Windows.Forms.SplitContainer splitContorl;
        private System.Windows.Forms.SplitContainer splitLeft;
        private Emgu.CV.UI.ImageBox pbZoom;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton pasteToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripButton cutToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripButton btnCanny;
    private System.Windows.Forms.ToolStrip thumbToolStrip;
    private System.Windows.Forms.ToolStripButton pbcFR;
    private System.Windows.Forms.ToolStripButton pbcStop;
    private System.Windows.Forms.ToolStripButton pbcPlay;
    private System.Windows.Forms.ToolStripButton pbcPlayPause;
    private System.Windows.Forms.ToolStripButton pbcFF;
    private System.Windows.Forms.ToolStripLabel pbcLabel;
    private System.Windows.Forms.ToolStripTextBox tbCurImage;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ComboBox cbSessioFPS;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ToolStripButton btnCacheAllFrames;
    private System.Windows.Forms.ToolStripSeparator cbSeperator;
    private System.Windows.Forms.CheckBox chkOverlayDiff;
  }
}

