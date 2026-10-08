#!/bin/sh
set -eu
cd "$(dirname "$0")/.."
test_output=$(mktemp -d "${TMPDIR:-/tmp}/mailify-tests.XXXXXX")
trap 'rm -rf "$test_output"' EXIT HUP INT TERM
if [ "${1:-}" = "--unit" ]; then
    set -- tests/ResourceDoubles.cs
else
    resgen Properties/Resources.resx "$test_output/MailFormatter.Properties.Resources.resources"
    set -- /define:REAL_RESOURCES "/resource:$test_output/MailFormatter.Properties.Resources.resources,MailFormatter.Properties.Resources.resources" Properties/Resources.Designer.cs tests/AssetTests.cs
fi
csc /nologo /warnaserror /out:"$test_output/RegressionTests.exe" /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Configuration.dll /r:System.Xml.Linq.dll "$@" HtmlTemplateHelper.cs Utils/ImageHelper.cs Properties/Settings.Designer.cs tests/RegressionTests.cs tests/OutlookDoubles.cs tests/AttachmentTests.cs Utils/AttachmentHelper.cs CommonEvents.cs tests/CommonEventTests.cs tests/SettingsTests.cs Utils/SettingsPersistence.cs Utils/HtmlBodyReader.cs
mono "$test_output/RegressionTests.exe"
# Compile the actual forms as well; runtime UI/COM integration still requires Windows Outlook.
csc /nologo /warnaserror /target:library /define:REAL_FORMS /out:"$test_output/FormsCompile.dll" /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Configuration.dll CommonEvents.cs HtmlTemplateHelper.cs Utils/ImageHelper.cs Utils/AttachmentHelper.cs Utils/HtmlBodyReader.cs Utils/SettingsPersistence.cs Properties/Settings.Designer.cs Properties/Resources.Designer.cs Forms/SettingsForm.cs Forms/SettingsForm.Designer.cs Forms/RawBodyForm.cs tests/OutlookDoubles.cs
