#pragma once

#include "uiMan.h"

namespace mainUI {

  using namespace System;
  using namespace System::ComponentModel;
  using namespace System::Collections;
  using namespace System::Windows::Forms;
  using namespace System::Data;
  using namespace System::Drawing;

  /// <summary>
  /// Summary for frmMainUI
  /// </summary>
  public ref class frmMainUI : public System::Windows::Forms::Form
  {
  public:
    frmMainUI(void)
    {
      InitializeComponent();
      //
      //TODO: Add the constructor code here
      //
    }

  public:
  private: System::Windows::Forms::ToolStripStatusLabel^  txtDebugMsg;
  private: System::Windows::Forms::SplitContainer^  splitContorl;
  private: Emgu::CV::UI::ImageBox^  pbOutView;
  private: System::Windows::Forms::ToolStrip^  toolStrip2;
  private: System::Windows::Forms::ToolStripButton^  btnSURF;
		   uiMan _uiMan;

  protected:
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    ~frmMainUI()
    {
      if (components)
      {
        delete components;
      }
    }
  private: System::Windows::Forms::StatusStrip^  statusStrip1;
  protected:
  private: System::Windows::Forms::ToolStrip^  toolStrip1;
  private: System::Windows::Forms::SplitContainer^  splitMain;

  private: System::Windows::Forms::SplitContainer^  splitLeft;
  private: System::Windows::Forms::SplitContainer^  splitRight;









  private: System::Windows::Forms::ListView^  lvThumbs;

  private: System::Windows::Forms::ToolStripStatusLabel^  toolStripStatusLabel1;
  private: System::Windows::Forms::ToolStripStatusLabel^  toolStripStatusLabel2;
  private: System::Windows::Forms::ToolStripStatusLabel^  toolStripStatusLabel3;
  private: System::Windows::Forms::ToolStripProgressBar^  toolStripProgressBar1;




  private: System::Windows::Forms::ToolStripSeparator^  toolStripSeparator1;
  private: System::Windows::Forms::StatusStrip^  statusStrip2;
  private: System::Windows::Forms::ToolStripStatusLabel^  txtPointerCoords;


  private: System::Windows::Forms::ToolStripButton^  newToolStripButton;
  private: System::Windows::Forms::ToolStripButton^  openToolStripButton;
  private: System::Windows::Forms::ToolStripButton^  saveToolStripButton;
  private: System::Windows::Forms::ToolStripButton^  printToolStripButton;
  private: System::Windows::Forms::ToolStripSeparator^  toolStripSeparator;
  private: System::Windows::Forms::ToolStripButton^  cutToolStripButton;
  private: System::Windows::Forms::ToolStripButton^  copyToolStripButton;
  private: System::Windows::Forms::ToolStripButton^  pasteToolStripButton;
  private: System::Windows::Forms::ToolStripSeparator^  toolStripSeparator2;
  private: System::Windows::Forms::ToolStripButton^  helpToolStripButton;
  private: Emgu::CV::UI::ImageBox^  pbMain;
  private: Emgu::CV::UI::ImageBox^  pbZoom;
  private: System::ComponentModel::IContainer^  components;

  private:
    /// <summary>
    /// Required designer variable.
    /// </summary>


#pragma region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
    void InitializeComponent(void)
    {
		this->components = (gcnew System::ComponentModel::Container());
		System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(frmMainUI::typeid));
		this->statusStrip1 = (gcnew System::Windows::Forms::StatusStrip());
		this->toolStripStatusLabel1 = (gcnew System::Windows::Forms::ToolStripStatusLabel());
		this->toolStripStatusLabel2 = (gcnew System::Windows::Forms::ToolStripStatusLabel());
		this->toolStripStatusLabel3 = (gcnew System::Windows::Forms::ToolStripStatusLabel());
		this->toolStripProgressBar1 = (gcnew System::Windows::Forms::ToolStripProgressBar());
		this->toolStrip1 = (gcnew System::Windows::Forms::ToolStrip());
		this->toolStripSeparator1 = (gcnew System::Windows::Forms::ToolStripSeparator());
		this->newToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->openToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->saveToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->printToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->toolStripSeparator = (gcnew System::Windows::Forms::ToolStripSeparator());
		this->cutToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->copyToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->pasteToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->toolStripSeparator2 = (gcnew System::Windows::Forms::ToolStripSeparator());
		this->helpToolStripButton = (gcnew System::Windows::Forms::ToolStripButton());
		this->splitMain = (gcnew System::Windows::Forms::SplitContainer());
		this->splitLeft = (gcnew System::Windows::Forms::SplitContainer());
		this->splitContorl = (gcnew System::Windows::Forms::SplitContainer());
		this->pbOutView = (gcnew Emgu::CV::UI::ImageBox());
		this->pbZoom = (gcnew Emgu::CV::UI::ImageBox());
		this->splitRight = (gcnew System::Windows::Forms::SplitContainer());
		this->pbMain = (gcnew Emgu::CV::UI::ImageBox());
		this->statusStrip2 = (gcnew System::Windows::Forms::StatusStrip());
		this->txtPointerCoords = (gcnew System::Windows::Forms::ToolStripStatusLabel());
		this->txtDebugMsg = (gcnew System::Windows::Forms::ToolStripStatusLabel());
		this->lvThumbs = (gcnew System::Windows::Forms::ListView());
		this->toolStrip2 = (gcnew System::Windows::Forms::ToolStrip());
		this->btnSURF = (gcnew System::Windows::Forms::ToolStripButton());
		this->statusStrip1->SuspendLayout();
		this->toolStrip1->SuspendLayout();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitMain))->BeginInit();
		this->splitMain->Panel1->SuspendLayout();
		this->splitMain->Panel2->SuspendLayout();
		this->splitMain->SuspendLayout();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitLeft))->BeginInit();
		this->splitLeft->Panel1->SuspendLayout();
		this->splitLeft->Panel2->SuspendLayout();
		this->splitLeft->SuspendLayout();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitContorl))->BeginInit();
		this->splitContorl->Panel2->SuspendLayout();
		this->splitContorl->SuspendLayout();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pbOutView))->BeginInit();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pbZoom))->BeginInit();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitRight))->BeginInit();
		this->splitRight->Panel1->SuspendLayout();
		this->splitRight->Panel2->SuspendLayout();
		this->splitRight->SuspendLayout();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pbMain))->BeginInit();
		this->statusStrip2->SuspendLayout();
		this->toolStrip2->SuspendLayout();
		this->SuspendLayout();
		// 
		// statusStrip1
		// 
		this->statusStrip1->ImageScalingSize = System::Drawing::Size(40, 40);
		this->statusStrip1->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(4) {
			this->toolStripStatusLabel1,
				this->toolStripStatusLabel2, this->toolStripStatusLabel3, this->toolStripProgressBar1
		});
		this->statusStrip1->Location = System::Drawing::Point(0, 999);
		this->statusStrip1->Name = L"statusStrip1";
		this->statusStrip1->Size = System::Drawing::Size(1971, 48);
		this->statusStrip1->TabIndex = 0;
		this->statusStrip1->Text = L"statusStrip1";
		// 
		// toolStripStatusLabel1
		// 
		this->toolStripStatusLabel1->Name = L"toolStripStatusLabel1";
		this->toolStripStatusLabel1->Size = System::Drawing::Size(297, 43);
		this->toolStripStatusLabel1->Text = L"toolStripStatusLabel1";
		// 
		// toolStripStatusLabel2
		// 
		this->toolStripStatusLabel2->Name = L"toolStripStatusLabel2";
		this->toolStripStatusLabel2->Size = System::Drawing::Size(297, 43);
		this->toolStripStatusLabel2->Text = L"toolStripStatusLabel2";
		// 
		// toolStripStatusLabel3
		// 
		this->toolStripStatusLabel3->Name = L"toolStripStatusLabel3";
		this->toolStripStatusLabel3->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->toolStripStatusLabel3->Size = System::Drawing::Size(960, 43);
		this->toolStripStatusLabel3->Spring = true;
		// 
		// toolStripProgressBar1
		// 
		this->toolStripProgressBar1->Alignment = System::Windows::Forms::ToolStripItemAlignment::Right;
		this->toolStripProgressBar1->Name = L"toolStripProgressBar1";
		this->toolStripProgressBar1->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->toolStripProgressBar1->Size = System::Drawing::Size(400, 42);
		// 
		// toolStrip1
		// 
		this->toolStrip1->AllowMerge = false;
		this->toolStrip1->ImageScalingSize = System::Drawing::Size(24, 24);
		this->toolStrip1->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(11) {
			this->toolStripSeparator1,
				this->newToolStripButton, this->openToolStripButton, this->saveToolStripButton, this->printToolStripButton, this->toolStripSeparator,
				this->cutToolStripButton, this->copyToolStripButton, this->pasteToolStripButton, this->toolStripSeparator2, this->helpToolStripButton
		});
		this->toolStrip1->Location = System::Drawing::Point(0, 0);
		this->toolStrip1->Name = L"toolStrip1";
		this->toolStrip1->Size = System::Drawing::Size(1971, 43);
		this->toolStrip1->Stretch = true;
		this->toolStrip1->TabIndex = 1;
		this->toolStrip1->Text = L"toolStrip1";
		// 
		// toolStripSeparator1
		// 
		this->toolStripSeparator1->AutoSize = false;
		this->toolStripSeparator1->Name = L"toolStripSeparator1";
		this->toolStripSeparator1->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->toolStripSeparator1->Size = System::Drawing::Size(40, 40);
		// 
		// newToolStripButton
		// 
		this->newToolStripButton->AutoSize = false;
		this->newToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->newToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"newToolStripButton.Image")));
		this->newToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->newToolStripButton->Name = L"newToolStripButton";
		this->newToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->newToolStripButton->Size = System::Drawing::Size(40, 40);
		this->newToolStripButton->Text = L"&New";
		this->newToolStripButton->Click += gcnew System::EventHandler(this, &frmMainUI::newToolStripButton_Click);
		// 
		// openToolStripButton
		// 
		this->openToolStripButton->AutoSize = false;
		this->openToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->openToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"openToolStripButton.Image")));
		this->openToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->openToolStripButton->Name = L"openToolStripButton";
		this->openToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->openToolStripButton->Size = System::Drawing::Size(40, 40);
		this->openToolStripButton->Text = L"&Open";
		// 
		// saveToolStripButton
		// 
		this->saveToolStripButton->AutoSize = false;
		this->saveToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->saveToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"saveToolStripButton.Image")));
		this->saveToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->saveToolStripButton->Name = L"saveToolStripButton";
		this->saveToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->saveToolStripButton->Size = System::Drawing::Size(40, 40);
		this->saveToolStripButton->Text = L"&Save";
		// 
		// printToolStripButton
		// 
		this->printToolStripButton->AutoSize = false;
		this->printToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->printToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"printToolStripButton.Image")));
		this->printToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->printToolStripButton->Name = L"printToolStripButton";
		this->printToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->printToolStripButton->Size = System::Drawing::Size(40, 40);
		this->printToolStripButton->Text = L"&Print";
		// 
		// toolStripSeparator
		// 
		this->toolStripSeparator->AutoSize = false;
		this->toolStripSeparator->Name = L"toolStripSeparator";
		this->toolStripSeparator->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->toolStripSeparator->Size = System::Drawing::Size(40, 40);
		// 
		// cutToolStripButton
		// 
		this->cutToolStripButton->AutoSize = false;
		this->cutToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->cutToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"cutToolStripButton.Image")));
		this->cutToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->cutToolStripButton->Name = L"cutToolStripButton";
		this->cutToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->cutToolStripButton->Size = System::Drawing::Size(40, 40);
		this->cutToolStripButton->Text = L"C&ut";
		// 
		// copyToolStripButton
		// 
		this->copyToolStripButton->AutoSize = false;
		this->copyToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->copyToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"copyToolStripButton.Image")));
		this->copyToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->copyToolStripButton->Name = L"copyToolStripButton";
		this->copyToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->copyToolStripButton->Size = System::Drawing::Size(40, 40);
		this->copyToolStripButton->Text = L"&Copy";
		// 
		// pasteToolStripButton
		// 
		this->pasteToolStripButton->AutoSize = false;
		this->pasteToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->pasteToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"pasteToolStripButton.Image")));
		this->pasteToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->pasteToolStripButton->Name = L"pasteToolStripButton";
		this->pasteToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->pasteToolStripButton->Size = System::Drawing::Size(40, 40);
		this->pasteToolStripButton->Text = L"&Paste";
		// 
		// toolStripSeparator2
		// 
		this->toolStripSeparator2->AutoSize = false;
		this->toolStripSeparator2->Name = L"toolStripSeparator2";
		this->toolStripSeparator2->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->toolStripSeparator2->Size = System::Drawing::Size(40, 40);
		// 
		// helpToolStripButton
		// 
		this->helpToolStripButton->AutoSize = false;
		this->helpToolStripButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Image;
		this->helpToolStripButton->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"helpToolStripButton.Image")));
		this->helpToolStripButton->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->helpToolStripButton->Name = L"helpToolStripButton";
		this->helpToolStripButton->Overflow = System::Windows::Forms::ToolStripItemOverflow::Never;
		this->helpToolStripButton->Size = System::Drawing::Size(40, 40);
		this->helpToolStripButton->Text = L"He&lp";
		// 
		// splitMain
		// 
		this->splitMain->Cursor = System::Windows::Forms::Cursors::Default;
		this->splitMain->Dock = System::Windows::Forms::DockStyle::Fill;
		this->splitMain->Location = System::Drawing::Point(0, 43);
		this->splitMain->Name = L"splitMain";
		// 
		// splitMain.Panel1
		// 
		this->splitMain->Panel1->Controls->Add(this->splitLeft);
		// 
		// splitMain.Panel2
		// 
		this->splitMain->Panel2->Controls->Add(this->splitRight);
		this->splitMain->Size = System::Drawing::Size(1971, 956);
		this->splitMain->SplitterDistance = 656;
		this->splitMain->TabIndex = 2;
		// 
		// splitLeft
		// 
		this->splitLeft->Dock = System::Windows::Forms::DockStyle::Fill;
		this->splitLeft->Location = System::Drawing::Point(0, 0);
		this->splitLeft->Name = L"splitLeft";
		this->splitLeft->Orientation = System::Windows::Forms::Orientation::Horizontal;
		// 
		// splitLeft.Panel1
		// 
		this->splitLeft->Panel1->Controls->Add(this->splitContorl);
		// 
		// splitLeft.Panel2
		// 
		this->splitLeft->Panel2->Controls->Add(this->pbZoom);
		this->splitLeft->Size = System::Drawing::Size(656, 956);
		this->splitLeft->SplitterDistance = 564;
		this->splitLeft->TabIndex = 0;
		// 
		// splitContorl
		// 
		this->splitContorl->Dock = System::Windows::Forms::DockStyle::Fill;
		this->splitContorl->Location = System::Drawing::Point(0, 0);
		this->splitContorl->Name = L"splitContorl";
		this->splitContorl->Orientation = System::Windows::Forms::Orientation::Horizontal;
		// 
		// splitContorl.Panel2
		// 
		this->splitContorl->Panel2->Controls->Add(this->pbOutView);
		this->splitContorl->Size = System::Drawing::Size(656, 564);
		this->splitContorl->SplitterDistance = 285;
		this->splitContorl->TabIndex = 0;
		// 
		// pbOutView
		// 
		this->pbOutView->AccessibleRole = System::Windows::Forms::AccessibleRole::Graphic;
		this->pbOutView->Dock = System::Windows::Forms::DockStyle::Fill;
		this->pbOutView->Location = System::Drawing::Point(0, 0);
		this->pbOutView->Name = L"pbOutView";
		this->pbOutView->Size = System::Drawing::Size(656, 275);
		this->pbOutView->TabIndex = 3;
		this->pbOutView->TabStop = false;
		// 
		// pbZoom
		// 
		this->pbZoom->AccessibleRole = System::Windows::Forms::AccessibleRole::Graphic;
		this->pbZoom->Dock = System::Windows::Forms::DockStyle::Fill;
		this->pbZoom->FunctionalMode = Emgu::CV::UI::ImageBox::FunctionalModeOption::Minimum;
		this->pbZoom->Location = System::Drawing::Point(0, 0);
		this->pbZoom->Margin = System::Windows::Forms::Padding(0);
		this->pbZoom->Name = L"pbZoom";
		this->pbZoom->Size = System::Drawing::Size(656, 388);
		this->pbZoom->SizeMode = System::Windows::Forms::PictureBoxSizeMode::CenterImage;
		this->pbZoom->TabIndex = 2;
		this->pbZoom->TabStop = false;
		// 
		// splitRight
		// 
		this->splitRight->Dock = System::Windows::Forms::DockStyle::Fill;
		this->splitRight->Location = System::Drawing::Point(0, 0);
		this->splitRight->Name = L"splitRight";
		this->splitRight->Orientation = System::Windows::Forms::Orientation::Horizontal;
		// 
		// splitRight.Panel1
		// 
		this->splitRight->Panel1->Controls->Add(this->toolStrip2);
		this->splitRight->Panel1->Controls->Add(this->pbMain);
		this->splitRight->Panel1->Controls->Add(this->statusStrip2);
		// 
		// splitRight.Panel2
		// 
		this->splitRight->Panel2->Controls->Add(this->lvThumbs);
		this->splitRight->Size = System::Drawing::Size(1311, 956);
		this->splitRight->SplitterDistance = 732;
		this->splitRight->TabIndex = 1;
		// 
		// pbMain
		// 
		this->pbMain->AccessibleRole = System::Windows::Forms::AccessibleRole::Graphic;
		this->pbMain->Dock = System::Windows::Forms::DockStyle::Fill;
		this->pbMain->Location = System::Drawing::Point(0, 0);
		this->pbMain->Name = L"pbMain";
		this->pbMain->Size = System::Drawing::Size(1311, 686);
		this->pbMain->TabIndex = 2;
		this->pbMain->TabStop = false;
		this->pbMain->Click += gcnew System::EventHandler(this, &frmMainUI::pbMain_Click);
		this->pbMain->DoubleClick += gcnew System::EventHandler(this, &frmMainUI::pbMain_DoubleClick);
		this->pbMain->MouseClick += gcnew System::Windows::Forms::MouseEventHandler(this, &frmMainUI::pbMain_MouseClick);
		this->pbMain->MouseDoubleClick += gcnew System::Windows::Forms::MouseEventHandler(this, &frmMainUI::pbMain_MouseDoubleClick);
		this->pbMain->MouseDown += gcnew System::Windows::Forms::MouseEventHandler(this, &frmMainUI::pbMain_MouseDown);
		this->pbMain->MouseMove += gcnew System::Windows::Forms::MouseEventHandler(this, &frmMainUI::pbMain_MouseMove);
		this->pbMain->MouseUp += gcnew System::Windows::Forms::MouseEventHandler(this, &frmMainUI::pbMain_MouseUp);
		// 
		// statusStrip2
		// 
		this->statusStrip2->ImageScalingSize = System::Drawing::Size(40, 40);
		this->statusStrip2->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(2) {
			this->txtPointerCoords,
				this->txtDebugMsg
		});
		this->statusStrip2->Location = System::Drawing::Point(0, 686);
		this->statusStrip2->Name = L"statusStrip2";
		this->statusStrip2->Size = System::Drawing::Size(1311, 46);
		this->statusStrip2->TabIndex = 1;
		this->statusStrip2->Text = L"statusStrip2";
		// 
		// txtPointerCoords
		// 
		this->txtPointerCoords->Name = L"txtPointerCoords";
		this->txtPointerCoords->Size = System::Drawing::Size(54, 41);
		this->txtPointerCoords->Text = L"///";
		// 
		// txtDebugMsg
		// 
		this->txtDebugMsg->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Text;
		this->txtDebugMsg->Name = L"txtDebugMsg";
		this->txtDebugMsg->Size = System::Drawing::Size(54, 41);
		this->txtDebugMsg->Text = L"///";
		this->txtDebugMsg->TextAlign = System::Drawing::ContentAlignment::MiddleRight;
		this->txtDebugMsg->TextImageRelation = System::Windows::Forms::TextImageRelation::TextBeforeImage;
		// 
		// lvThumbs
		// 
		this->lvThumbs->Alignment = System::Windows::Forms::ListViewAlignment::Left;
		this->lvThumbs->AutoArrange = false;
		this->lvThumbs->Dock = System::Windows::Forms::DockStyle::Fill;
		this->lvThumbs->GridLines = true;
		this->lvThumbs->HeaderStyle = System::Windows::Forms::ColumnHeaderStyle::None;
		this->lvThumbs->HideSelection = false;
		this->lvThumbs->Location = System::Drawing::Point(0, 0);
		this->lvThumbs->MultiSelect = false;
		this->lvThumbs->Name = L"lvThumbs";
		this->lvThumbs->ShowGroups = false;
		this->lvThumbs->Size = System::Drawing::Size(1311, 220);
		this->lvThumbs->TabIndex = 0;
		this->lvThumbs->UseCompatibleStateImageBehavior = false;
		// 
		// toolStrip2
		// 
		this->toolStrip2->ImageScalingSize = System::Drawing::Size(40, 40);
		this->toolStrip2->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(1) { this->btnSURF });
		this->toolStrip2->Location = System::Drawing::Point(0, 0);
		this->toolStrip2->Name = L"toolStrip2";
		this->toolStrip2->Size = System::Drawing::Size(1311, 48);
		this->toolStrip2->TabIndex = 3;
		this->toolStrip2->Text = L"toolStrip2";
		// 
		// btnSURF
		// 
		this->btnSURF->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Text;
		this->btnSURF->Image = (cli::safe_cast<System::Drawing::Image^>(resources->GetObject(L"btnSURF.Image")));
		this->btnSURF->ImageTransparentColor = System::Drawing::Color::Magenta;
		this->btnSURF->Name = L"btnSURF";
		this->btnSURF->Size = System::Drawing::Size(92, 45);
		this->btnSURF->Text = L"SURF";
		this->btnSURF->Click += gcnew System::EventHandler(this, &frmMainUI::btnSURF_Click);
		// 
		// frmMainUI
		// 
		this->AutoScaleDimensions = System::Drawing::SizeF(16, 31);
		this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
		this->BackgroundImageLayout = System::Windows::Forms::ImageLayout::None;
		this->ClientSize = System::Drawing::Size(1971, 1047);
		this->Controls->Add(this->splitMain);
		this->Controls->Add(this->toolStrip1);
		this->Controls->Add(this->statusStrip1);
		this->Name = L"frmMainUI";
		this->Text = L"Davidi";
		this->FormClosing += gcnew System::Windows::Forms::FormClosingEventHandler(this, &frmMainUI::frmMainUI_FormClosing);
		this->Load += gcnew System::EventHandler(this, &frmMainUI::frmMainUI_Load);
		this->Shown += gcnew System::EventHandler(this, &frmMainUI::frmMainUI_Shown);
		this->statusStrip1->ResumeLayout(false);
		this->statusStrip1->PerformLayout();
		this->toolStrip1->ResumeLayout(false);
		this->toolStrip1->PerformLayout();
		this->splitMain->Panel1->ResumeLayout(false);
		this->splitMain->Panel2->ResumeLayout(false);
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitMain))->EndInit();
		this->splitMain->ResumeLayout(false);
		this->splitLeft->Panel1->ResumeLayout(false);
		this->splitLeft->Panel2->ResumeLayout(false);
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitLeft))->EndInit();
		this->splitLeft->ResumeLayout(false);
		this->splitContorl->Panel2->ResumeLayout(false);
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitContorl))->EndInit();
		this->splitContorl->ResumeLayout(false);
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pbOutView))->EndInit();
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pbZoom))->EndInit();
		this->splitRight->Panel1->ResumeLayout(false);
		this->splitRight->Panel1->PerformLayout();
		this->splitRight->Panel2->ResumeLayout(false);
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->splitRight))->EndInit();
		this->splitRight->ResumeLayout(false);
		(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pbMain))->EndInit();
		this->statusStrip2->ResumeLayout(false);
		this->statusStrip2->PerformLayout();
		this->toolStrip2->ResumeLayout(false);
		this->toolStrip2->PerformLayout();
		this->ResumeLayout(false);
		this->PerformLayout();

	}
#pragma endregion
  private: System::Void frmMainUI_Load(System::Object^  sender, System::EventArgs^  e)
  {
  }
  private: System::Void newToolStripButton_Click(System::Object^  sender, System::EventArgs^  e)
  {
    OpenFileDialog ^folderBrowserDialog1 = gcnew  OpenFileDialog();
    folderBrowserDialog1->Multiselect = false;
    folderBrowserDialog1->CheckPathExists = true;
    folderBrowserDialog1->RestoreDirectory = true;
    Object^ oFolder = Application::UserAppDataRegistry->GetValue("browseFolder", System::IO::Path::GetDirectoryName(Application::ExecutablePath));
    folderBrowserDialog1->InitialDirectory = (String^)oFolder;
    if (folderBrowserDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK)
    {
      Application::UserAppDataRegistry->SetValue("browseFolder", System::IO::Path::GetDirectoryName(folderBrowserDialog1->FileName));
      _uiMan.createNewSet(pbMain, folderBrowserDialog1->FileName);//"D:\\davidi\\Nachshon\\1\\Gerstenfeld_nachshon_3_Camera_0013.tif");
    }
    delete folderBrowserDialog1;
  }
  private: System::Void frmMainUI_Shown(System::Object^  sender, System::EventArgs^  e)
  {
    Object^ oTop = Application::UserAppDataRegistry->GetValue("wndTop", Top);
    Object^ oLeft = Application::UserAppDataRegistry->GetValue("wndLeft", Left);
    Object^ oW = Application::UserAppDataRegistry->GetValue("wndW", Size.Width);
    Object^ oH = Application::UserAppDataRegistry->GetValue("wndH", Size.Height);
    Object^ leftSplitPos = Application::UserAppDataRegistry->GetValue("leftSplitPos", splitLeft->SplitterDistance);
    Object^ rightSplitPos = Application::UserAppDataRegistry->GetValue("rightSplitPos", splitRight->SplitterDistance);
    Object^ mainSplitPos = Application::UserAppDataRegistry->GetValue("mainSplitPos", splitMain->SplitterDistance);
    Object^ splitContorlPos = Application::UserAppDataRegistry->GetValue("prevSplitPos", splitContorl->SplitterDistance);
    Width = (int)oW;
    Height = (int)oH;
    Left = (int)oLeft;
    Top = (int)oTop;
    splitLeft->SplitterDistance = (int)leftSplitPos;
    splitRight->SplitterDistance = (int)rightSplitPos;
    splitMain->SplitterDistance = (int)mainSplitPos;
    splitContorl->SplitterDistance = (int)splitContorlPos;
  }
  private: System::Void frmMainUI_FormClosing(System::Object^  sender, System::Windows::Forms::FormClosingEventArgs^  e)
  {
    Application::UserAppDataRegistry->SetValue("wndTop", Top);
    Application::UserAppDataRegistry->SetValue("wndLeft", Left);
    Application::UserAppDataRegistry->SetValue("wndW", Size.Width);
    Application::UserAppDataRegistry->SetValue("wndH", Size.Height);
    Application::UserAppDataRegistry->SetValue("leftSplitPos", splitLeft->SplitterDistance);
    Application::UserAppDataRegistry->SetValue("rightSplitPos", splitRight->SplitterDistance);
    Application::UserAppDataRegistry->SetValue("mainSplitPos", splitMain->SplitterDistance);
    Application::UserAppDataRegistry->SetValue("prevSplitPos", splitContorl->SplitterDistance);
  }
  private: System::Void pbMain_MouseMove(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e)
  {
	  Point^ mp = gcnew Point(e->X, e->Y);
	  Point^ ip = gcnew Point(e->X, e->Y);
    if (_uiMan.pbToImgCoord(pbMain, e->X, e->Y, ip))
    {
      txtPointerCoords->Text = "(" + Convert::ToString(ip->X) + "," + Convert::ToString(ip->Y) + ")";
	    _uiMan.updateOverlayAndZoom(pbMain, pbZoom, mp, ip);

      switch (_uiMan.dragPhase)
      {
      default:
      case 0:
        break;
      case 1:
        txtDebugMsg->Text = "Selection: {" + _uiMan.p1.ToString() + ", " + ip->ToString() + ", ...: ";
        break;
      case 2:
        txtDebugMsg->Text = "Selection: {" + _uiMan.p1.ToString() + ", " + _uiMan.p2.ToString() + ", " + ip->ToString() + "}";
        break;
      }
    }

  }
  private: System::Void pbMain_MouseDown(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e)
  {
    Point^ ip = gcnew Point(e->X, e->Y);
    if (e->Button == System::Windows::Forms::MouseButtons::Right)
    {
      if (_uiMan.dragPhase > 0)
      {
        _uiMan.dragPhase = 0;
        txtDebugMsg->Text = "";
      }
    }
    else if (e->Button == System::Windows::Forms::MouseButtons::Left && _uiMan.pbToImgCoord(pbMain, e->X, e->Y, ip))
    {
      switch (_uiMan.dragPhase)
      {
      default:
        break;
      case 0:
        _uiMan.p1 = *ip;
        _uiMan.dragPhase = 1;
        txtDebugMsg->Text = "Selection: {" + _uiMan.p1.ToString() + ", ...: ";
        break;
      case 1:
        _uiMan.p2 = *ip;
        _uiMan.dragPhase = 2;
        txtDebugMsg->Text = "Selection: {" + _uiMan.p1.ToString() + ", " + _uiMan.p2.ToString() + ", ...: ";
        break;
      case 2:
        _uiMan.p3 = *ip;
        _uiMan.dragPhase = 0;
	      _uiMan.drawPreview(pbMain, pbOutView, _uiMan.p1, _uiMan.p2, _uiMan.p3);
        txtDebugMsg->Text = "Selection: {" + _uiMan.p1.ToString() + ", " + _uiMan.p2.ToString() + ", " + _uiMan.p3.ToString() + "}";
        break;
      }
    }
  }
  private: System::Void pbMain_MouseUp(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e)
  {
  }
  private: System::Void pbMain_DoubleClick(System::Object^  sender, System::EventArgs^  e)
  {
  }
  private: System::Void pbMain_Click(System::Object^  sender, System::EventArgs^  e)
  {
  }
  private: System::Void pbMain_MouseDoubleClick(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e)
  {
    /*
    if (pbMain->SizeMode == PictureBoxSizeMode::Normal)
    {
      pbMain->SizeMode = PictureBoxSizeMode::Zoom;
      pbMain->FunctionalMode = Emgu::CV::UI::ImageBox::FunctionalModeOption::Minimum;
      pbMain->HorizontalScrollBar->Value = 0;
      pbMain->VerticalScrollBar->Value = 0;
    }
    else
    {
      pbMain->SizeMode = PictureBoxSizeMode::Normal;
      pbMain->FunctionalMode = Emgu::CV::UI::ImageBox::FunctionalModeOption::Everything;
    }
    */
  }
  private: System::Void pbMain_MouseClick(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e)
  {
  }
  private: System::Void btnSURF_Click(System::Object^  sender, System::EventArgs^  e)
  {
	  _uiMan.SURF(pbMain);
  }
};
}
