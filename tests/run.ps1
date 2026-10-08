$ErrorActionPreference = 'Stop'
Push-Location (Join-Path $PSScriptRoot '..')
$testOutput = Join-Path ([System.IO.Path]::GetTempPath()) ('mailify-tests-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testOutput | Out-Null
try {
    $resource = Join-Path $testOutput 'MailFormatter.Properties.Resources.resources'
    & resgen Properties/Resources.resx $resource
    if ($LASTEXITCODE -ne 0) { throw 'Resource compilation failed' }
    $sources = @('HtmlTemplateHelper.cs', 'Utils/ImageHelper.cs', 'Properties/Settings.Designer.cs', 'Properties/Resources.Designer.cs', 'tests/RegressionTests.cs', 'tests/OutlookDoubles.cs', 'tests/AttachmentTests.cs', 'Utils/AttachmentHelper.cs', 'CommonEvents.cs', 'tests/CommonEventTests.cs', 'tests/SettingsTests.cs', 'Utils/SettingsPersistence.cs', 'Utils/HtmlBodyReader.cs', 'tests/AssetTests.cs')
    $binary = Join-Path $testOutput 'RegressionTests.exe'
    & csc /nologo /warnaserror /define:REAL_RESOURCES "/out:$binary" /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Configuration.dll /r:System.Xml.Linq.dll "/resource:$resource,MailFormatter.Properties.Resources.resources" $sources
    if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed' }
    & $binary
    if ($LASTEXITCODE -ne 0) { throw 'Regression tests failed' }
} finally {
    Remove-Item -LiteralPath $testOutput -Recurse -Force
    Pop-Location
}
