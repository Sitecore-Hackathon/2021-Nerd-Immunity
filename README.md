# Hackathon Submission Entry form

## Team name
⟹ Nerd Immunity

## Category
⟹ The best enhancement to the Sitecore Admin (XP) for Content Editors & Marketers

## Description
  - Our entry is a Sitecore module that allows easy access in the Sitecore editors to change Cloudflare cache settings on single items or purge Cloudflare caches for an entire site.
  - What problem was solved (if any): Cloudflare cache and page settings are not readily available in Sitecore Editors. This allows access to freely set page rules and purge Site caches.


## Video link
⟹ Provide a video highlighing your Hackathon module submission and provide a link to the video. You can use any video hosting, file share or even upload the video to this repository. _Just remember to update the link below_

⟹ [Replace this Video link](#video-link)



## Pre-requisites and Dependencies

- Cloudflare CDN
- Sitecore 10.1
- Docker


## Installation instructions
⟹ Write a short clear step-wise instruction on how to install your module.  

> _A simple well-described installation process is required to win the Hackathon._  
> Feel free to use any of the following tools/formats as part of the installation:
> - Sitecore Package files
> - Docker image builds
> - Sitecore CLI
> - msbuild
> - npm / yarn
> 
> _Do not use_
> - TDS
> - Unicorn
 
f. ex. 

1. Start docker environment using `.\Start-Hackathon.ps1`
2. Open solution in Visual Studio and run build
3. Use the Sitecore Installation wizard to install the [package](#link-to-package)
4. ...
5. profit

### Configuration
⟹ If there are any custom configuration that has to be set manually then remember to add all details here.

_Remove this subsection if your entry does not require any configuration that is not fully covered in the installation instructions already_

## Usage instructions
-To get started, you first need to configure each site and it's corresponding Cloudflare Settings by creating 'Site' items</p>
![Module Start](docs/images/modules.jpg?raw=true "Module Start")

-Insert a 'Site' Item for each domain cache you are trying to configure.
![Insert Your Sites](docs/images/insert-site.jpg?raw=true "Insert Your Sites")

⟹ Site Items require the following field data:
1. Site Start Item : This should be corresponding 'Home' or Start Item for the Site you are configuring
2. Delivery Target Hostname : The Site's full domain that is cached by Cloudflare should be entered here. (ex. https://www.sitecorehackathon.org/)
3. CF Token : The Site's corresponding Cloudflare API Token should be entered here.
4. CF Zone ID : The Site's corresponding Zone ID should be entered here.


![Site Item Fields](docs/images/site-item.jpg?raw=true "Site Item Fields")

-After Sites have been configured, you can use these two module features:
  -The publish ribbon has a new 'Purge Cloudflare Site Cache' button.
  -This feature will clear the entire site cache for the selected item's corresponding Site. (Defined in the Site Start Item chosen above)

![Full Site Cache Clear](docs/images/site-clear.jpg?raw=true "Full Site Cache Clear")

⟹ Each Item in the Content Tree now has a Cloudflare Section
-Purge Full Cache for this Item will set a Page Rule in Cloudflare to cache and purge the entire page on publish.
  -The CDN 'Page Rule' is set on save. A check or uncheck will look for a corresponding page rule and either add or remove from Cloudflare settings.
-Page Rule ID : IMPORTANT: Do Not Edit this Field.

![Single Item Cache Rule](docs/images/item-page-rule.jpg?raw=true "Single Item Cache Rule")



## Comments

