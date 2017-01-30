#include "frmMainUI.h"

using namespace System;
using namespace System::Windows::Forms;

using namespace mainUI;

[STAThreadAttribute]
void Main(array<String^>^ args)
{
  Application::EnableVisualStyles();
  Application::SetCompatibleTextRenderingDefault(false);

  mainUI::frmMainUI form;
  Application::Run(%form);
}
