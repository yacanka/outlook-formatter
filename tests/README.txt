Mailify regression checks

Mono/C# compiler: sh tests/run.sh
Fast core-only checks (no image I/O): sh tests/run.sh --unit
Windows Visual Studio Developer PowerShell: ./tests/run.ps1
No new package dependency is required. Tests write build artifacts only to a unique temporary folder.

Default checks compile the real Resources.resx, load every embedded image, decode every
project image, and run production HTML, event, attachment and settings code. Outlook,
VSTO Ribbon and modal preview surfaces use explicit doubles because these checks do
not launch Outlook. Settings persistence tests inject a failing storage implementation.
The shell runner additionally compiles the actual WinForms classes with VSTO doubles.
These checks are not a full VSTO build or a substitute for Windows integration tests.

Required Windows follow-up:
- Build Mailify.sln in Visual Studio with Office/VSTO development tools and .NET 4.7.2.
- From Explorer, format HTML with tables, links and inline CID images; inspect the new
  draft, reopen it and confirm that source content and attachment bytes are unchanged.
- Format plain text containing literal HTML. Confirm it remains visible as plain text.
- Format a compose message twice, including after save/reopen. The second application
  must leave the first template unchanged. Verify the leading bookmark survives the
  installed Outlook version's HTML normalization. Replies with an older formatted
  quotation must still accept their own formatting.
- Open source preview, copy text, close, switch selection and reopen. No mail content,
  body format or attachments should change. The preview text box must be read-only.
- Try saving settings to an unwritable profile. The dialog must stay open, previous
  in-memory settings/Ribbon visibility remain, and retry must work once storage recovers.
- Inject/trigger an attachment read or metadata write failure. No partial draft should
  be displayed and temporary files should be removed; source attachments must remain.
- Check all ten templates in the receiving Outlook client; data-URI rendering and
  Outlook HTML normalization vary by client and are not established by these tests.

Behavior choices:
- Missing/malformed body tags or unsupported template index return original HTML.
- Reapplying formatting to a body with its leading Mailify bookmark is a no-op. Change
  the template before first formatting; template replacement is not implemented.
- Footer is plain text, HTML-encoded, with line breaks preserved.
- Only MAPI_E_NOT_FOUND is tolerated when reading optional inline metadata. Other
  read/write failures abort copying instead of silently losing inline image references.
- Public legacy helper methods are retained for compatibility; no public API removal.

Images:
34 project images were generated with the built-in image_gen tool from filename meanings.
Prompts: Resources/generation-prompts.json. Preview: Resources/preview.html.
They are inferred replacement artwork, not recovered originals or official branding.
Images were proportionally downscaled for email/UI use; transparent icon alpha is retained.
