---
---

# Show Blazor Page Load Progress

**Today I created a Blazor WebAssembly app that shows a progressbar while the page loads.** This is especially useful for users on slow connections because Blazor apps typically require several megabytes of DLL files to be downloaded before meaningful content appears on the page.

Live Demo: https://swharden.com/LJPcalc/

## Add a progress bar

Edit index.html and identify your app's main div:

```html
<div id="app">Loading...</div>
```

Add a progress bar inside it:
```html
<div id="app">
    <h3>Loading...</h3>
    <div class="progress" style="height: 2em;">
        <div id="progressbar" class="progress-bar text-start px-2" role="progressbar" style="width: 0%;"></div>
    </div>
</div>
```

See [Bootstrap's progressbar page](https://getbootstrap.com/docs/5.2/components/progress/) for extensive customization and animation options and best practices when working with progress indicators. Also ensure the version of Bootstrap in your Blazor app is consistent with the documentation/HTML you are referencing.

## Disable Blazor AutoStart

Edit index.html and identify where your app loads Blazor resources:

```html
<script src="_framework/blazor.webassembly.js"></script>
```

Update that script so it does _not_ download automatically:
```html
<script src="_framework/blazor.webassembly.js" autostart="false"></script>
```

## Create a Blazor Startup Script

Add a script to the bottom of the page to start Blazor manually, identifying all the resources needed and incrementally downloading them while updating the progressbar along the way.

```html
<script>
    function StartBlazor() {
        let numberOfResourcesLoaded = 0;
        const resourcesToLoad = [];
        Blazor.start({
            loadBootResource:
                function (type, filename, defaultUri, integrity) {
                    if (type == "dotnetjs")
                        return defaultUri;

                    const fetchResources = fetch(defaultUri, {
                        cache: 'no-cache',
                        integrity: integrity,
                        headers: { 'MyCustomHeader': 'My custom value' }
                    });

                    resourcesToLoad.push(fetchResources);

                    fetchResources.then((r) => {
                        numberOfResourcesLoaded += 1;
                        const resourceCount = resourcesToLoad.length;
                        const percentLoaded = parseInt((numberOfResourcesLoaded * 100.0) / resourceCount);
                        const progressbar = document.getElementById('progressbar');
                        progressbar.style.width = percentLoaded + '%';
                        progressbar.innerText = `${numberOfResourcesLoaded}/${resourceCount} (${percentLoaded}%) ${filename}`;
                    });

                    return fetchResources;
                }
        });
    }

    StartBlazor();
</script>
```

## Resources

* [ASP.NET Core Blazor startup](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup) describes the `Blazor.start()` process and `loadBootResource()`.

* [@EdmondShtogu's comment](https://github.com/dotnet/aspnetcore/issues/25165#issuecomment-781683925) on [dotnet/aspnetcore #25165](https://github.com/dotnet/aspnetcore/issues/25165) from Feb 18, 2021