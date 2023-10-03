![Build succeeded][build-shield]
![Test passing][test-shield]
[![Issues][issues-shield]][issues-url]
[![Issues][closed-shield]][issues-url]
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![License][license-shield]][license-url]

# UBER
#### Ultimativ Befordrings Effektiviserings Rapportering
<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>

- [Case](#case)
- [Requirements](#requirements)
- [Architecture diagram](#architecture-diagram)
- [Roadmap](#roadmap)
- [Summary and rundown](#summary-and-rundown)
- [Getting started](#getting-started)
- [API endpoints](#api-endpoints)
- [Libraries](#libraries)
- [Database diagram](#database-diagram)
  - [Alarm](#alarm-1)
- [Flowcharts](#flowcharts)
  - [Alarm](#alarm-2)
- [License](#license)
- [Contact](#contact)
</details>

# Case
Inspireret af udenlandske eksempler ønsker IT-Center Syd at udvikle og implementere en række nye IT-services:
> Fælles Transportplanlægning:
> 
>  En applikation, der hjælper elever og personale med at organisere samkørsel eller fælles transportmuligheder.
 Dette kan bidrage til at reducere antallet af individuelle bilture til skolen og dermed mindske CO2-udledningen.
<p align="right">(<a href="#top">back to top</a>)</p>

# Requirements
- [X] Lave webapplikation der kan vise medstuderende/ansattes bopæl
- [X] Vise rute fra bopæl til skole
- [x] Formidle kontakt mellem brugere med henblik på samkørsel
- [x] Benytte login for at sikre data
- [X] Adskille database/api/frontend
<p align="right">(<a href="#top">back to top</a>)</p>

# Architecture diagram
![architecture diagram](/Docs/Architecture_Diagram.png)
`/Docs/Architecture_Diagram.png`

# Roadmap
- [X] Vise kort med markers
  - [x] Vise rute fra "mig" til skole
- [X] Kun vise relevante brugere
  - Frasorterer de brugere der ønsker et lift, hvis man selv ønsker lift
- [ ] Integrere med eksisterende Azure AD/stamdata
  - Dette var ikke muligt grundet sikkerhedsspørgsmål fra ITC Syd
- [X] Vise *proof of concept* vha. simuleret elev-data


#  Summary and rundown
UBER er lavet som et proof of concept, hvor elever og ansatte kan sørge for samkørsel.
Meningen er at man som bruger af systemet kan tage stilling til om man vil køre eller have et lift af en anden medstuderende
eller ansat. 

Når man har valgt, f.eks. at man vil have et lift, vil man på et kort kunne se sin direkte rute til skolen, samt se en markør 
med navn på de andre der tilbyder at køre med. 

Der kan man så anmode om samkørsel, og den bruger man har valgt vil på sin side kunne se en liste over anmodninger, samt acceptere en anmodning.
> Peter har ikke selv bil, og vil gerne have muligheden for at køre med en i skolen. Han kan se via systemet at Jan bor længere væk, og at ruten går forbi
> der hvor Peter bor. 
> 
> Peter anmoder derfor Jan om at kunne køre med, og da Jan næste gang logger ind på systemet vil han i en liste kunne se at Peter har anmodet om samkørsel.
> Jan kan også på et kort se hvor Peter bor ift. hans rute, og han vælger derfor at acceptere anmodningen.
<p align="right">(<a href="#top">back to top</a>)</p>


# Getting started
For at komme igang med programmet, kræver det tre steps.
1. Databaseopsætning
   1. Installer database jf. SQL script der findes under `/setup`
   2. Opret bruger og giv de korrekte privileges
2. API opsætning
   1. Opsæt API med korrekt connection til databasen
   2. Noter adresse og evt. port på API
3. Frontend opsætning
   1. Inden opstart skal API'ens base URI eventuelt rettes. Den findes under `/Yber.Blazor/appsettings.json`
```json
  "YberAPIBaseURI": "https://yber-api.tved.it/"
```
<p align="right">(<a href="#top">back to top</a>)</p>



# API endpoints<sup>1</sup>
| Topics              | Params                           | Method     |
|:--------------------|:---------------------------------|:-----------|
| /GetStudentLift     | none                             | GET        |
| /GetStudentDriver   | none                             | GET        |
| /GetStudentRoute    | studentUsername                  | POST       |
| /RequestLift        | studentUsername, studentUsername | POST       |
| /AcceptLift         | studentUsername, studentUsername | POST       |
| /ViewLifts          | studentUsername                  | POST       |
| /WantLift           | studentId                        | POST       |
| /OfferDrive         | studentId                        | POST       |
| /GetStudentsFromID  | studentId                        | POST       |
| /GetStudentFromName | studentUsername                  | GET        |
# Libraries
## Yber.API
| Name                                              | Version |
| :------------------------------------------------ | :------ |
| Azure.Extensions.AspNetCore.Configuration.Secrets | 1.0.0   |
| Azure.Identity                                    | 1.6.0   |
| Microsoft.AspNetCore.Authentication.JwtBearer     | 7.0.11  |
| Microsoft.AspNetCore.Authentication.OpenIdConnect | 7.0.11  |
| Microsoft.Identity.Web                            | 2.13.4  |
| Microsoft.Identity.Web.UI                         | 2.13.4  |
| Swashbuckle.AspNetCore                            | 6.5.0   |
| Seq.Extensions.Logging                            | 6.1.0   |
<p align="right">(<a href="#top">back to top</a>)</p>

## Yber.Blazor
| Name                                              | Version |
| :------------------------------------------------ | :------ |
| Azure.Extensions.AspNetCore.Configuration.Secrets | 1.0.0   |
| Azure.Identity                                    | 1.6.0   |
| Blazored.Modal                                    | 7.1.0   |
| Blazored.Toast                                    | 4.1.0   |
| Microsoft.AspNetCore.Authentication.OpenIdConnect | 7.0.11  |
| Microsoft.Identity.Web                            | 2.13.4  |
| Microsoft.Identity.Web.MicrosoftGraph             | 2.13.4  |
| Microsoft.Identity.Web.UI                         | 2.13.4  |
| Radzen.Blazor                                     | 4.15.14 |
| Seq.Extensions.Logging                            | 6.1.0   |
| Toolbelt.Blazor.HotKeys2                          | 3.0.0   |
| Toolbelt.Blazor.I18nText                          | 12.0.2  |
<p align="right">(<a href="#top">back to top</a>)</p>

## Yber.Repositories
| Name                             | Version |
| :------------------------------- | :------ |
| GoogleApi                        | 5.2.1   |
| Microsoft.EntityFrameworkCore    | 7.0.11  |
| Pomelo.EntityFrameworkCore.MySql | 7.0.0   |
<p align="right">(<a href="#top">back to top</a>)</p>


# Database Diagram

![DB Diagram.png](DOCS%2FDB%20Diagram.png)
<p align="right">(<a href="#top">back to top</a>)</p>

# Flowcharts

## Alarm
![alarm flowchart](/Docs/Alarm_Flowchart.png)
`/Docs/Alarm_Flowchart.png`
<p align="right">(<a href="#top">back to top</a>)</p>


# License
* Hardware: CC-BY-LA (Creative Commons)
* API: GPLv3
* Frontend: GPLv3
<p align="right">(<a href="#top">back to top</a>)</p>

# Contact
- Peter Hymøller - peterhym21@gmail.com
  - [![Twitter][twitter-shield-ptr]][twitter-url-ptr]
- Nicolai Heuck - nicolaiheuck@gmail.com
- Jan Andreasen - jan@tved.it
  - [![Twitter][twitter-shield]][twitter-url]

Project Link: [https://github.com/nicolaiheuck/Yber.Blazor](https://github.com/nicolaiheuck/Yber.Blazor)
<p align="right">(<a href="#top">back to top</a>)</p>

<sup>1</sup> - See swagger for more info


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[build-shield]: https://img.shields.io/badge/Build-passed-brightgreen.svg
[test-shield]: https://img.shields.io/badge/Tests-passed-brightgreen.svg
[contributors-shield]: https://img.shields.io/github/contributors/nicolaiheuck/Yber.Blazor.svg?style=badge
[contributors-url]: https://github.com/nicolaiheuck/Yber.Blazor/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/nicolaiheuck/Yber.Blazor.svg?style=badge
[forks-url]: https://github.com/nicolaiheuck/Yber.Blazor/network/members
[issues-shield]: https://img.shields.io/github/issues/nicolaiheuck/Yber.Blazor.svg?style=badge
[closed-shield]: https://img.shields.io/github/issues-closed/nicolaiheuck/Yber.Blazor?label=%20
[issues-url]: https://github.com/nicolaiheuck/Yber.Blazor/issues
[license-shield]: https://img.shields.io/github/license/nicolaiheuck/Yber.Blazor.svg?style=badge
[license-url]: https://github.com/nicolaiheuck/Yber.Blazor/blob/master/LICENSE
[twitter-shield]: https://img.shields.io/twitter/follow/andreasen_jan?style=social
[twitter-url]: https://twitter.com/andreasen_jan
[twitter-shield-ptr]: https://img.shields.io/twitter/follow/peter_hym?style=social
[twitter-url-ptr]: https://twitter.com/peter_hym
