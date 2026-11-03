# DEAL (Declarative and Extensible Abstract Language) Environment
DEAL is a **declarative**, **user-friend**, **deterministic**, **explicit**, **hybrid**, **interoperable**, **modular**, **extensible**, **abstract**, **reusable**, declarative language designed for deterministic compilation into executable target languages, (JavaScript for DEAL.JS). That represents computational instructions using constrained natural-language-like syntax while preserving unrestricted target machine language interoperability and supporting an extensible vocabulary.
* DEAL is understandable by humans, generatable by AI, and compilable by the machine.
* DEAL instructions will parse directly into complex and optimized pure target machine language code to use everywhere.
* You can write pure target machine language between your instructions freely.

Start DEAL.JS directly using the tutorials [here](https://github.com/DEAL-X/JS/blob/main/README.md)...

## Browser prerequisite

Start Chrome or Edge with remote debugging enabled. For example:

```powershell
# Close all Chrome processes first, then:
& "$env:ProgramFiles\Google\Chrome\Application\chrome.exe" `
  --remote-debugging-port=9222 `
  --user-data-dir="$env:LOCALAPPDATA\DEAL\ChromeProfile"
```

The profile directory is intentionally separate from the user's everyday profile. Chromium may refuse remote debugging against the default profile in recent versions. Keep the debugging endpoint bound to localhost; do not expose port 9222 to an untrusted network.

## JavaScript examples

```javascript
const response = await fetch("https://example.com/");
console.log(response.status, response.ok);
const html = await response.text();

const posted = await fetch("https://example.com/api", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name: "DEAL" })
});
```

```javascript
const page = await load("https://example.com/");
console.log(page.Title);
console.log(await page.EvaluateAsync("document.title"));
console.log(await page.GetDocumentHtmlAsync());
```

## Compatibility and explicit limitations

This implementation deliberately provides a native JavaScript-facing Fetch shape over .NET `HttpClient`, but it is not a browser networking stack. In particular:

- CORS, service workers, browser cache, browser cookie partitioning, browser credentials policy and browser `Request` modes are not implemented by `HttpClient`.
- The initial implementation buffers response bodies. `ReadableStream`, streaming uploads/downloads, `duplex`, and true backpressure are not implemented.
- `redirect: "follow"` is supported by the transport's default handler. `manual` and `error` are not yet implemented with full browser semantics.
- `AbortSignal` cancellation is bridged through the injected host object's `Abort` method; host cancellation/timeout exceptions are surfaced as rejected promises.
- `FormData`, `Blob`, URLSearchParams bodies, browser-managed cookies, and opaque responses require further parity work before claiming full browser Fetch conformance.
- Browser page objects are remote CDP proxies, not local DOM `Window` objects. Use `EvaluateAsync`, `GetDocumentHtmlAsync`, and `GetDocumentTextAsync` to access the remote page.
- `load()` creates a new tab in the already-running Chromium instance. It does not silently attach to an arbitrary existing tab.
- CDP's `/json/new` endpoint and remote-debugging behavior vary by Chromium version. Keep the browser endpoint configurable and localhost-only.

The browser connection uses the Chrome DevTools Protocol over WebSocket and does not add Playwright, Selenium, or a second browser runtime.
