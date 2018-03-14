# Moov2.Orchard.Azure.PassThrough

[Orchard](http://www.orchardproject.net/) module that enhances the functionality of the existing Orchard.Azure module. The primary goal of this module is to let content admins secure their azure stored media instead of having to having all URLs for media publically available. This module uses a Pass Through approach to secure media. Essentially we expose a new route that users will be served media via that will ensure they have the new `ViewSecureMedia` permission before serving the media. This module can serve media from a private Azure blob storage container.

## Status

*Currently under development.*

## Getting Set Up

Download module source code and place within the "Modules" directory of your Orchard installation.

Or download the .nupkg file from the releases area.

Alternatively, use the command below to add this module as a sub-module within your Orchard project.

    git submodule add git@github.com:moov2/Moov2.Orchard.Azure.PassThrough.git modules/Moov2.Orchard.Azure.PassThrough

# Usage

Enable the "Moov2.Orchard.Azure.PassThrough" module.

Ensure that existing HTML content media URLs are replaced with `/SecureMedia/` versions:

Where you would have `Media` in the URL for local Orchard media you would now have `SecureMedia`.

Alternatively for Azure media you would replace `*blob endpoint*/media/` with `/SecureMedia/`.