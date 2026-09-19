using HSLR.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Data
{
    public static class HsrlDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HsrlDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // 1. Seed Admin Role and User
            const string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            const string adminEmail = "admin@hsrl.uet.edu.pk";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "HsrlAdmin@2026!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            // 2. Seed Site Configuration
            if (!await context.SiteConfigs.AnyAsync())
            {
                context.SiteConfigs.Add(new SiteConfig
                {
                    SiteName = "Hydroclimatic Sensing Research Lab (HSRL)",
                    Tagline = "Satellite Remote Sensing, Hydrological Modeling & Climate Analytics",
                    Description = "The Hydroclimatic Sensing Research Lab (HSRL) is an interdisciplinary research laboratory associated with the Centre of Excellence in Water Resources Engineering (CEWRE), University of Engineering and Technology (UET) Lahore. HSRL leverages spaceborne observation, advanced numerical modeling, and AI/ML algorithms to address critical water security, disaster risk, and climate resilience challenges across the Indus Basin and beyond.",
                    DirectorName = "Prof. Dr. Muhammad Waseem Boota",
                    ContactEmail = "hsrl@uet.edu.pk",
                    Phone = "+92-42-99029200",
                    Location = "Centre of Excellence in Water Resources Engineering (CEWRE), UET Lahore, GT Road, Lahore 54890, Pakistan",
                    Affiliation = "Centre of Excellence in Water Resources Engineering (CEWRE), University of Engineering and Technology (UET) Lahore",
                    TwitterUrl = "https://twitter.com/hsrl_uet",
                    LinkedInUrl = "https://linkedin.com/company/hsrl-uet",
                    GoogleScholarUrl = "https://scholar.google.com",
                    GitHubUrl = "https://github.com/hsrl-uet"
                });
                await context.SaveChangesAsync();
            }

            // 3. Seed Research Domains
            if (!await context.ResearchDomains.AnyAsync())
            {
                var d1 = new ResearchDomain
                {
                    Slug = "satellite-hydrology",
                    Title = "Satellite Remote Sensing & Earth Observation",
                    ShortDescription = "Harnessing multi-sensor radar (SAR), optical, and gravimetric spaceborne data to observe terrestrial water storage, soil moisture, and river hydraulics.",
                    Description = "Our lab develops algorithms and processing workflows to transform high-resolution satellite imagery and radar products into operational hydrologic datasets. By fusing radar altimetry, Sentinel Synthetic Aperture Radar (SAR), GPM precipitation constellations, and GRACE/GRACE-FO gravity observations, we quantify water flux and balance across ungauged and inaccessible catchments.",
                    IconName = "bi-broadcast",
                    Featured = true,
                    DisplayOrder = 1,
                    FocusTopics = new List<ResearchDomainFocusTopic>
                    {
                        new() { Topic = "Synthetic Aperture Radar (SAR) Flood Extent Mapping", DisplayOrder = 1 },
                        new() { Topic = "Satellite Radar Altimetry for Inland River and Reservoir Water Levels", DisplayOrder = 2 },
                        new() { Topic = "High-Resolution Soil Moisture Estimation from SMAP and Sentinel-1", DisplayOrder = 3 },
                        new() { Topic = "Multi-Satellite Quantitative Precipitation Estimation (GPM IMERG)", DisplayOrder = 4 }
                    }
                };

                var d2 = new ResearchDomain
                {
                    Slug = "flood-drought-dynamics",
                    Title = "Hydroclimatic Extremes & Flood Risk Dynamics",
                    ShortDescription = "Predicting, mapping, and mitigating the compounding hazards of extreme monsoon floods, river inundation, and agricultural flash droughts.",
                    Description = "We combine physics-informed 2D hydrodynamic models with spaceborne earth observation to delineate flood propagation, assess breach scenarios, and evaluate community vulnerability. Furthermore, we develop multi-index drought monitoring frameworks tracking standardized soil moisture and vegetation health across agricultural belts.",
                    IconName = "bi-cloud-rain-heavy",
                    Featured = true,
                    DisplayOrder = 2,
                    FocusTopics = new List<ResearchDomainFocusTopic>
                    {
                        new() { Topic = "Physics-based 2D Inundation & Dam-Breach Modeling", DisplayOrder = 1 },
                        new() { Topic = "Near-Real-Time Flood Hazard Forecasting & Warning Systems", DisplayOrder = 2 },
                        new() { Topic = "Compound Flash Drought & Heatwave Risk Analysis", DisplayOrder = 3 },
                        new() { Topic = "Climate-Induced Loss & Damage Assessments for Infrastructure", DisplayOrder = 4 }
                    }
                };

                var d3 = new ResearchDomain
                {
                    Slug = "cryosphere-glacier-hydrology",
                    Title = "Cryosphere & High-Mountain Glacier Hydrology",
                    ShortDescription = "Monitoring glacier mass balance, snow-cover dynamics, and Glacier Lake Outburst Floods (GLOFs) in the Hindu Kush-Himalaya and Karakoram ranges.",
                    Description = "The Upper Indus Basin relies heavily on snow and glacier melt. HSRL conducts cutting-edge research tracking Karakoram glacier anomalies, debris-covered glacier ablation dynamics, and seasonal snow-water equivalent (SWE) using satellite photogrammetry and energy-balance glacio-hydrological models.",
                    IconName = "bi-snow2",
                    Featured = true,
                    DisplayOrder = 3,
                    FocusTopics = new List<ResearchDomainFocusTopic>
                    {
                        new() { Topic = "Glacier Lake Outburst Flood (GLOF) Early Warning & Hazard Zoning", DisplayOrder = 1 },
                        new() { Topic = "Satellite Geodetic Glacier Mass Balance Calculations", DisplayOrder = 2 },
                        new() { Topic = "Snow-Water Equivalent (SWE) Retrieval from Spaceborne Microwave", DisplayOrder = 3 },
                        new() { Topic = "Upper Indus River Runoff Sensitivity to Climate Warming", DisplayOrder = 4 }
                    }
                };

                var d4 = new ResearchDomain
                {
                    Slug = "ai-ml-water-forecasting",
                    Title = "AI, Deep Learning & Predictive Water Analytics",
                    ShortDescription = "Pioneering Physics-Informed Neural Networks (PINNs), LSTM streamflow forecasting, and computer vision models for hydroclimatic sensing.",
                    Description = "HSRL integrates modern deep learning architectures with physical conservation laws. We employ Long Short-Term Memory (LSTM) networks, spatial transformers, and graph neural networks to predict flash floods, forecast reservoir inflows, and downscale coarse climate model projections to local catchment scales.",
                    IconName = "bi-cpu",
                    Featured = true,
                    DisplayOrder = 4,
                    FocusTopics = new List<ResearchDomainFocusTopic>
                    {
                        new() { Topic = "Physics-Informed Deep Learning for Hydrological Streamflow Forecasting", DisplayOrder = 1 },
                        new() { Topic = "Computer Vision for Automated Water Body & River Bank Detection", DisplayOrder = 2 },
                        new() { Topic = "Downscaling Global Climate Models (CMIP6) via Generative AI", DisplayOrder = 3 },
                        new() { Topic = "Intelligent Decision Support for Multi-Reservoir Operation", DisplayOrder = 4 }
                    }
                };

                var d5 = new ResearchDomain
                {
                    Slug = "groundwater-depletion-grace",
                    Title = "Groundwater Dynamics & GRACE Satellite Sensing",
                    ShortDescription = "Tracking groundwater depletion, aquifer stress, and irrigated agriculture water extraction across the Indus Alluvial Plain.",
                    Description = "Groundwater is the lifeline of agricultural security in Pakistan. HSRL utilizes GRACE/GRACE-FO satellite gravimetry coupled with in-situ piezometric network observations and regional groundwater numerical modeling (MODFLOW) to map rates of aquifer depletion and salinization.",
                    IconName = "bi-layers-half",
                    Featured = true,
                    DisplayOrder = 5,
                    FocusTopics = new List<ResearchDomainFocusTopic>
                    {
                        new() { Topic = "GRACE/GRACE-FO Terrestrial Water & Groundwater Storage Disaggregation", DisplayOrder = 1 },
                        new() { Topic = "Regional Aquifer Depletion & Recharge Modeling in the Indus Basin", DisplayOrder = 2 },
                        new() { Topic = "Satellite Evapotranspiration (SEBAL/METRIC) & Crop Water Consumption", DisplayOrder = 3 },
                        new() { Topic = "Aquifer Salinization & Sustainable Pumping Regimes", DisplayOrder = 4 }
                    }
                };

                context.ResearchDomains.AddRange(d1, d2, d3, d4, d5);
                await context.SaveChangesAsync();
            }

            // 4. Seed Capabilities
            if (!await context.Capabilities.AnyAsync())
            {
                context.Capabilities.AddRange(
                    new Capability
                    {
                        Category = "Earth Observation",
                        Title = "Satellite Data Processing & Earth Observation Engine",
                        Description = "Automated high-throughput cloud ingestion pipelines utilizing Google Earth Engine, AWS Open Data, and European Space Agency Copernicus Open Access Hub for optical, radar (SAR), and altimetry missions.",
                        IconName = "bi-globe-americas",
                        DisplayOrder = 1,
                        Highlights = new List<CapabilityHighlight>
                        {
                            new() { Highlight = "Multi-temporal Sentinel-1 SAR interferometry and backscatter analysis", DisplayOrder = 1 },
                            new() { Highlight = "Google Earth Engine distributed cloud processing clusters", DisplayOrder = 2 },
                            new() { Highlight = "Automated pipeline for GPM, MODIS, Landsat 8/9, and Sentinel-2 ingestion", DisplayOrder = 3 }
                        }
                    },
                    new Capability
                    {
                        Category = "Numerical Modeling",
                        Title = "Hydrodynamic & Hydrological Simulation Suite",
                        Description = "Coupled 1D/2D hydraulic and watershed simulation environments calibrated to the Indus River Basin, tributary floodplains, and high-altitude alpine headwaters.",
                        IconName = "bi-diagram-3",
                        DisplayOrder = 2,
                        Highlights = new List<CapabilityHighlight>
                        {
                            new() { Highlight = "2D unsteady flow simulation using HEC-RAS, TUFLOW, and Delft3D", DisplayOrder = 1 },
                            new() { Highlight = "Distributed hydrologic modeling with WRF-Hydro, SWAT+, and VIC", DisplayOrder = 2 },
                            new() { Highlight = "Integrated surface-groundwater coupling via MODFLOW-OWHM", DisplayOrder = 3 }
                        }
                    },
                    new Capability
                    {
                        Category = "Artificial Intelligence",
                        Title = "Deep Learning & Scientific Machine Learning Workstations",
                        Description = "GPU-accelerated deep learning computing lab dedicated to training Physics-Informed Neural Networks, spatio-temporal LSTMs, and automated satellite image segmentation.",
                        IconName = "bi-cpu-fill",
                        DisplayOrder = 3,
                        Highlights = new List<CapabilityHighlight>
                        {
                            new() { Highlight = "Dual NVIDIA RTX A6000 GPU deep learning inference servers", DisplayOrder = 1 },
                            new() { Highlight = "PyTorch, JAX, and TensorFlow scientific computing pipelines", DisplayOrder = 2 },
                            new() { Highlight = "Real-time streamflow and flood inundation forecast inference APIs", DisplayOrder = 3 }
                        }
                    },
                    new Capability
                    {
                        Category = "Field & In-Situ Sensing",
                        Title = "Field Hydrometric & In-Situ Ground Truth Validation",
                        Description = "Precision field instrumentation for in-situ hydro-meteorological measurements, drone-based aerial multispectral surveys, and sensor telemetry.",
                        IconName = "bi-geo-alt",
                        DisplayOrder = 4,
                        Highlights = new List<CapabilityHighlight>
                        {
                            new() { Highlight = "Acoustic Doppler Current Profilers (ADCP) for river discharge gauging", DisplayOrder = 1 },
                            new() { Highlight = "Multispectral and thermal sensor payload drones for reach-scale surveys", DisplayOrder = 2 },
                            new() { Highlight = "In-situ FDR soil moisture probe telemetry networks", DisplayOrder = 3 }
                        }
                    }
                );
                await context.SaveChangesAsync();
            }

            // Retrieve domains for foreign key links
            var domains = await context.ResearchDomains.ToListAsync();
            var satDomain = domains.First(d => d.Slug == "satellite-hydrology");
            var floodDomain = domains.First(d => d.Slug == "flood-drought-dynamics");
            var cryoDomain = domains.First(d => d.Slug == "cryosphere-glacier-hydrology");
            var aiDomain = domains.First(d => d.Slug == "ai-ml-water-forecasting");
            var gwDomain = domains.First(d => d.Slug == "groundwater-depletion-grace");

            // 5. Seed People
            if (!await context.People.AnyAsync())
            {
                var director = new Person
                {
                    Slug = "prof-dr-muhammad-waseem-boota",
                    Name = "Prof. Dr. Muhammad Waseem Boota",
                    DisplayName = "Prof. Dr. Muhammad Waseem",
                    PhotoUrl = "/images/team/director.jpg",
                    Role = "Professor & Director, HSRL",
                    Category = PersonCategory.LabDirector,
                    Status = PersonStatus.Active,
                    ShortBio = "Professor of Water Resources Engineering at CEWRE, UET Lahore. Specializes in satellite hydrology, hydroclimatic extremes, and predictive water resource modeling across the Indus Basin.",
                    Biography = @"Prof. Dr. Muhammad Waseem Boota is a distinguished researcher and educator in water resources engineering, remote sensing, and hydroclimatic sciences. He serves as Director of the Hydroclimatic Sensing Research Lab (HSRL) and Professor at the Centre of Excellence in Water Resources Engineering (CEWRE), University of Engineering and Technology (UET) Lahore.

With over two decades of experience in academic leadership, research supervision, and policy consultation, Prof. Waseem has spearheaded pioneering studies on Indus Basin hydrology, transboundary river modeling, satellite earth observation, and climate change impacts. His research group collaborates internationally with leading earth observation agencies, UNESCO chair networks, and water research consortia.

Under his guidance, HSRL has established cutting-edge research facilities bridging numerical modeling with spaceborne satellite sensing, producing dozens of high-impact journal publications and training the next generation of Pakistani water scientists and GIS specialists.",
                    CurrentAffiliation = "Centre of Excellence in Water Resources Engineering (CEWRE), UET Lahore",
                    Featured = true,
                    Initials = "MWB",
                    GoogleScholarUrl = "https://scholar.google.com",
                    ResearchGateUrl = "https://researchgate.net",
                    LinkedInUrl = "https://linkedin.com",
                    OrcidUrl = "https://orcid.org/0000-0002-0000-0000",
                    DisplayOrder = 1,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "Satellite Remote Sensing & Radar Hydrology", DisplayOrder = 1 },
                        new() { Interest = "Indus Basin Hydroclimatic Extremes & Flood Risk", DisplayOrder = 2 },
                        new() { Interest = "High-Mountain Cryosphere & Alpine Water Resources", DisplayOrder = 3 },
                        new() { Interest = "Data-Driven Hydrological Modeling & AI", DisplayOrder = 4 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = satDomain.Id },
                        new() { ResearchDomainId = floodDomain.Id },
                        new() { ResearchDomainId = cryoDomain.Id }
                    }
                };

                var r1 = new Person
                {
                    Slug = "dr-usman-ali-tahir",
                    Name = "Dr. Usman Ali Tahir",
                    DisplayName = "Dr. Usman Ali",
                    PhotoUrl = "/images/team/usman-ali.jpg",
                    Role = "Senior Postdoctoral Research Fellow",
                    Category = PersonCategory.Researcher,
                    Status = PersonStatus.Active,
                    ShortBio = "Specialist in machine learning applications in hydrology, physics-informed neural networks, and satellite precipitation estimation.",
                    Biography = "Dr. Usman Ali completed his PhD in Water Resources Engineering with a focus on deep learning streamflow forecasting and satellite radar precipitation retrieval. At HSRL, he leads the AI and Scientific Computing division.",
                    CurrentAffiliation = "CEWRE, UET Lahore",
                    Featured = true,
                    Initials = "UAT",
                    GoogleScholarUrl = "https://scholar.google.com",
                    ResearchGateUrl = "https://researchgate.net",
                    DisplayOrder = 2,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "Physics-Informed Deep Learning", DisplayOrder = 1 },
                        new() { Interest = "Satellite Precipitation Downscaling", DisplayOrder = 2 },
                        new() { Interest = "River Basin Forecast Models", DisplayOrder = 3 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = aiDomain.Id },
                        new() { ResearchDomainId = satDomain.Id }
                    }
                };

                var r2 = new Person
                {
                    Slug = "engr-fatima-zahra",
                    Name = "Engr. Fatima Zahra",
                    DisplayName = "Engr. Fatima Zahra",
                    PhotoUrl = "/images/team/fatima-zahra.jpg",
                    Role = "Research Associate & GIS Specialist",
                    Category = PersonCategory.Researcher,
                    Status = PersonStatus.Active,
                    ShortBio = "Hydrological GIS analyst specializing in Copernicus Sentinel SAR flood extent processing and 2D hydraulic flood propagation models.",
                    Biography = "Engr. Fatima holds an MS in Hydrology and Water Resources Engineering. Her research centers on automated satellite flood delineation and hydraulic bridge scour modeling.",
                    CurrentAffiliation = "CEWRE, UET Lahore",
                    Featured = true,
                    Initials = "FZ",
                    LinkedInUrl = "https://linkedin.com",
                    DisplayOrder = 3,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "Sentinel-1 SAR Interferometry & Flood Mapping", DisplayOrder = 1 },
                        new() { Interest = "2D Unsteady Hydraulic Modeling (HEC-RAS)", DisplayOrder = 2 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = floodDomain.Id },
                        new() { ResearchDomainId = satDomain.Id }
                    }
                };

                var phd1 = new Person
                {
                    Slug = "bilal-ahmed-khan",
                    Name = "Engr. Bilal Ahmed Khan",
                    DisplayName = "Bilal A. Khan",
                    Role = "PhD Research Scholar",
                    Category = PersonCategory.Phd,
                    Status = PersonStatus.Active,
                    ShortBio = "Investigating Upper Indus Basin cryospheric changes, glacier mass balance, and GLOF dynamics using multi-temporal spaceborne stereoscopy.",
                    CurrentAffiliation = "CEWRE, UET Lahore",
                    Featured = false,
                    Initials = "BAK",
                    DisplayOrder = 4,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "Glacier Mass Balance & Altimetry", DisplayOrder = 1 },
                        new() { Interest = "GLOF Hazard Zoning in Northern Pakistan", DisplayOrder = 2 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = cryoDomain.Id }
                    }
                };

                var ms1 = new Person
                {
                    Slug = "zainab-noor",
                    Name = "Zainab Noor",
                    DisplayName = "Zainab Noor",
                    Role = "MS Water Resources Engineering Candidate",
                    Category = PersonCategory.MsMphil,
                    Status = PersonStatus.Active,
                    ShortBio = "Researching groundwater storage variations across the Rechna Doab using GRACE-FO gravimetry and local piezometer bore networks.",
                    CurrentAffiliation = "CEWRE, UET Lahore",
                    Featured = false,
                    Initials = "ZN",
                    DisplayOrder = 5,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "GRACE/GRACE-FO Groundwater Storage Tracking", DisplayOrder = 1 },
                        new() { Interest = "Aquifer Salinization in Irrigated Plains", DisplayOrder = 2 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = gwDomain.Id }
                    }
                };

                var alumni1 = new Person
                {
                    Slug = "dr-hamza-tariq",
                    Name = "Dr. Hamza Tariq",
                    DisplayName = "Dr. Hamza Tariq",
                    Role = "PhD Graduate (2024)",
                    Category = PersonCategory.Alumni,
                    Status = PersonStatus.Alumni,
                    ShortBio = "Former HSRL PhD Scholar; currently Postdoctoral Researcher in Hydrological Remote Sensing at University of Bristol, UK.",
                    CurrentAffiliation = "School of Geographical Sciences, University of Bristol, UK",
                    Featured = false,
                    Initials = "HT",
                    DisplayOrder = 6,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "Satellite Altimetry for Global Rivers", DisplayOrder = 1 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = satDomain.Id }
                    }
                };

                var collab1 = new Person
                {
                    Slug = "dr-stephen-miller",
                    Name = "Prof. Dr. Stephen Miller",
                    DisplayName = "Prof. Stephen Miller",
                    Role = "International Visiting Collaborator",
                    Category = PersonCategory.Collaborator,
                    Status = PersonStatus.Collaborator,
                    ShortBio = "Senior Research Scientist in Space Geodesy and Hydrology at ETH Zurich; collaborative investigator on Indus cryosphere monitoring.",
                    CurrentAffiliation = "Institute of Geodesy and Photogrammetry, ETH Zurich",
                    Featured = false,
                    Initials = "SM",
                    DisplayOrder = 7,
                    ResearchInterests = new List<PersonResearchInterest>
                    {
                        new() { Interest = "Satellite Radar & Cryospheric Geodesy", DisplayOrder = 1 }
                    },
                    PersonResearchAreas = new List<PersonResearchArea>
                    {
                        new() { ResearchDomainId = cryoDomain.Id }
                    }
                };

                context.People.AddRange(director, r1, r2, phd1, ms1, alumni1, collab1);
                await context.SaveChangesAsync();
            }

            // 6. Seed Projects
            if (!await context.Projects.AnyAsync())
            {
                var p1 = new Project
                {
                    Slug = "indus-sar-flood-risk-forecasting",
                    Title = "Spaceborne Earth Observation for Transboundary Indus Basin Flood Risk Forecasting",
                    ShortTitle = "Indus SAR Flood Forecasting",
                    Subtitle = "Integrating Sentinel-1 SAR, SWOT altimetry, and 2D hydrodynamics for operational floodplain inundation prediction",
                    Status = ProjectStatus.Active,
                    ProjectType = ProjectType.Research,
                    Year = 2025,
                    StudyArea = "Indus River Basin (Kashmore to Thatta Reach)",
                    Country = "Pakistan",
                    Challenge = "The Lower Indus Basin suffers recurring, catastrophic monsoon floods characterized by extensive embankment breaches and prolonged inundation. Traditional stream gauges frequently malfunction during extreme stages, leaving disaster management authorities without accurate spatial estimates of floodwater velocity and water depth.",
                    Impact = "Delivered an automated near-real-time flood mapping tool now piloted with regional disaster response stakeholders, providing 10-meter resolution flood inundation depth maps within 3 hours of satellite overpass.",
                    Featured = true,
                    ImageUrl = "/images/projects/flood-project.jpg",
                    DisplayOrder = 1,
                    Objectives = new List<ProjectObjective>
                    {
                        new() { Objective = "Develop an automated SAR cloud-penetrating flood detection algorithm on Google Earth Engine", DisplayOrder = 1 },
                        new() { Objective = "Couple spaceborne water surface elevations with 2D hydrodynamic HEC-RAS model calibrations", DisplayOrder = 2 },
                        new() { Objective = "Provide real-time flood risk bulletins for vulnerable downstream districts in Sindh and Punjab", DisplayOrder = 3 }
                    },
                    DataSources = new List<ProjectDataSource>
                    {
                        new() { SourceName = "Copernicus Sentinel-1 SAR C-Band Level-1 GRD", DisplayOrder = 1 },
                        new() { SourceName = "Surface Water and Ocean Topography (SWOT) Ka-Band Altimeter", DisplayOrder = 2 },
                        new() { SourceName = "ALOS PALSAR 12.5m High-Resolution Digital Elevation Model (DEM)", DisplayOrder = 3 }
                    },
                    MethodologySteps = new List<ProjectMethodologyStep>
                    {
                        new() { StepTitle = "SAR Pre-processing & Radiometric Calibration", StepDescription = "Thermal noise removal, radiometric calibration, and Lee sigma speckle filtering on SAR backscatter time series.", DisplayOrder = 1 },
                        new() { StepTitle = "Bimodal Otsu Thresholding & Water Masking", StepDescription = "Dynamic threshold estimation separating smooth specular open water reflections from surrounding roughened vegetation.", DisplayOrder = 2 },
                        new() { StepTitle = "Hydrodynamic Inundation Inversion", StepDescription = "Water surface profile elevation integration with high-resolution DEMs to calculate pixel-level flood water depths.", DisplayOrder = 3 }
                    },
                    Technologies = new List<ProjectTechnology>
                    {
                        new() { TechnologyName = "Google Earth Engine API (Python)", DisplayOrder = 1 },
                        new() { TechnologyName = "HEC-RAS 2D Unsteady Hydraulic Engine", DisplayOrder = 2 },
                        new() { TechnologyName = "GDAL / Rasterio Geospatial Stack", DisplayOrder = 3 }
                    },
                    KeyFindings = new List<ProjectKeyFinding>
                    {
                        new() { Finding = "Achieved 91.4% overall classification accuracy against ground validation points during the Indus flood crisis.", DisplayOrder = 1 },
                        new() { Finding = "Identified critical secondary embankment failure zones that had been missed by standard 1D hydraulic gauges.", DisplayOrder = 2 }
                    },
                    ProjectResearchAreas = new List<ProjectResearchArea>
                    {
                        new() { ResearchDomainId = satDomain.Id },
                        new() { ResearchDomainId = floodDomain.Id }
                    }
                };

                var p2 = new Project
                {
                    Slug = "grace-indus-aquifer-depletion",
                    Title = "GRACE and Sentinel-Based Depletion Tracking of the Indus Alluvial Aquifer",
                    ShortTitle = "Indus Aquifer Depletion Tracking",
                    Subtitle = "Multi-decadal satellite gravimetry and machine learning estimation of groundwater withdrawal in the Punjab breadbasket",
                    Status = ProjectStatus.Ongoing,
                    ProjectType = ProjectType.Applied,
                    Year = 2024,
                    StudyArea = "Indus Basin Irrigation System (IBIS) — Punjab & Sindh",
                    Country = "Pakistan",
                    Challenge = "With over 1.2 million agricultural tube wells operating in Pakistan, excessive unregulated pumping is causing alarming water table drops and brackish groundwater upconing across intensive cropping zones.",
                    Impact = "Supplied evidence-based depletion rates to national irrigation authorities, supporting the development of provincial groundwater regulatory policy guidelines.",
                    Featured = true,
                    ImageUrl = "/images/projects/groundwater-project.jpg",
                    DisplayOrder = 2,
                    Objectives = new List<ProjectObjective>
                    {
                        new() { Objective = "Isolate groundwater mass loss from GRACE/GRACE-FO terrestrial water storage anomalies", DisplayOrder = 1 },
                        new() { Objective = "Reconcile satellite estimates with thousands of in-situ piezometer observations across the doabs", DisplayOrder = 2 }
                    },
                    DataSources = new List<ProjectDataSource>
                    {
                        new() { SourceName = "GRACE & GRACE-Follow On Mascon Solutions (CSR & JPL)", DisplayOrder = 1 },
                        new() { SourceName = "Global Land Data Assimilation System (GLDAS Catchment LSM)", DisplayOrder = 2 },
                        new() { SourceName = "Provincial Irrigation Department Piezometric Water Level Logs", DisplayOrder = 3 }
                    },
                    MethodologySteps = new List<ProjectMethodologyStep>
                    {
                        new() { StepTitle = "Terrestrial Water Storage Mass Budget Decomposition", StepDescription = "Subtracting soil moisture, surface water, and canopy storage modeled by GLDAS from GRACE gravity anomalies.", DisplayOrder = 1 },
                        new() { StepTitle = "Random Forest Spatial Downscaling", StepDescription = "Downscaling 0.5-degree mascon footprints to 1km resolution using vegetation indices, thermal inertia, and elevation.", DisplayOrder = 2 }
                    },
                    Technologies = new List<ProjectTechnology>
                    {
                        new() { TechnologyName = "Python Scientific Stack (NumPy, SciPy, Scikit-learn)", DisplayOrder = 1 },
                        new() { TechnologyName = "MODFLOW 6 Aquifer Simulator", DisplayOrder = 2 },
                        new() { TechnologyName = "QGIS Automated Scripting", DisplayOrder = 3 }
                    },
                    KeyFindings = new List<ProjectKeyFinding>
                    {
                        new() { Finding = "Discovered an average annual groundwater depletion of 1.35 cm/year equivalent water height in the Upper Punjab doabs.", DisplayOrder = 1 }
                    },
                    ProjectResearchAreas = new List<ProjectResearchArea>
                    {
                        new() { ResearchDomainId = gwDomain.Id },
                        new() { ResearchDomainId = satDomain.Id }
                    }
                };

                var p3 = new Project
                {
                    Slug = "karakoram-glof-cryosphere-hazard",
                    Title = "Cryospheric Melt and Glacier Lake Outburst Flood (GLOF) Vulnerability in Northern Pakistan",
                    ShortTitle = "Karakoram GLOF Vulnerability",
                    Subtitle = "Multi-temporal satellite photogrammetry and energy-balance glacio-hydrological modeling in Gilgit-Baltistan",
                    Status = ProjectStatus.Active,
                    ProjectType = ProjectType.Collaborative,
                    Year = 2025,
                    StudyArea = "Hunza, Gilgit & Shigar River Valleys (Karakoram Range)",
                    Country = "Pakistan",
                    Challenge = "Rapidly expanding moraine-dammed and supraglacial lakes in the Karakoram pose catastrophic sudden-drainage GLOF threats to vulnerable mountain communities, roads, and Karakoram Highway infrastructure.",
                    Impact = "Identified 36 potentially dangerous glacial lakes, producing standardized bathymetric risk maps used by disaster risk management units.",
                    Featured = true,
                    ImageUrl = "/images/projects/glacier-project.jpg",
                    DisplayOrder = 3,
                    Objectives = new List<ProjectObjective>
                    {
                        new() { Objective = "Map decadal expansion rates of glacial lakes using PlanetScope and Sentinel-2 imagery", DisplayOrder = 1 },
                        new() { Objective = "Simulate moraine dam breach hydrographs and downstream flood wave travel times", DisplayOrder = 2 }
                    },
                    DataSources = new List<ProjectDataSource>
                    {
                        new() { SourceName = "PlanetScope 3-meter Daily Constellation Imagery", DisplayOrder = 1 },
                        new() { SourceName = "Copernicus Sentinel-2 MSI Multi-spectral Imagery", DisplayOrder = 2 },
                        new() { SourceName = "TanDEM-X High-Resolution Glacier Surface DEMs", DisplayOrder = 3 }
                    },
                    MethodologySteps = new List<ProjectMethodologyStep>
                    {
                        new() { StepTitle = "Deep-Learning Lake Delineation", StepDescription = "Convolutional neural network segmentation trained on high-altitude moraine and ice lakes.", DisplayOrder = 1 },
                        new() { StepTitle = "Moraine Dam Piping & Overtopping Simulation", StepDescription = "Hydrodynamic breach initiation modeling predicting peak outflow volumes.", DisplayOrder = 2 }
                    },
                    Technologies = new List<ProjectTechnology>
                    {
                        new() { TechnologyName = "PyTorch Deep Learning Segmentation", DisplayOrder = 1 },
                        new() { TechnologyName = "BASEMENT 2D Hydrodynamic Dam Break Solver", DisplayOrder = 2 }
                    },
                    KeyFindings = new List<ProjectKeyFinding>
                    {
                        new() { Finding = "Documented a 28% increase in supraglacial lake surface area across the surveyed glaciated valleys between 2015 and 2024.", DisplayOrder = 1 }
                    },
                    ProjectResearchAreas = new List<ProjectResearchArea>
                    {
                        new() { ResearchDomainId = cryoDomain.Id },
                        new() { ResearchDomainId = aiDomain.Id }
                    }
                };

                var p4 = new Project
                {
                    Slug = "ai-pinn-streamflow-forecasting",
                    Title = "Physics-Informed Deep Learning for Ungauged Tributary Streamflow Forecasting",
                    ShortTitle = "Physics-Informed AI Streamflow",
                    Subtitle = "Coupling recurrent neural networks with water conservation equations for real-time mountain runoff prediction",
                    Status = ProjectStatus.Completed,
                    ProjectType = ProjectType.Thesis,
                    Year = 2024,
                    StudyArea = "Jhelum & Chenab River Catchments",
                    Country = "Pakistan",
                    Challenge = "Data scarcity in steep mountainous catchments results in poor calibration for standard empirical rainfall-runoff models, causing unreliability in dam reservoir inflow management.",
                    Impact = "Demonstrated that Physics-Informed Neural Networks improve Nash-Sutcliffe Efficiency (NSE) by 14% compared to standard unconstrained LSTM models in ungauged basin scenarios.",
                    Featured = false,
                    ImageUrl = "/images/projects/ai-project.jpg",
                    DisplayOrder = 4,
                    Objectives = new List<ProjectObjective>
                    {
                        new() { Objective = "Embed physical mass conservation loss penalties into bidirectional LSTM architectures", DisplayOrder = 1 }
                    },
                    DataSources = new List<ProjectDataSource>
                    {
                        new() { SourceName = "ERA5-Land Reanalysis Hourly Hydrometeorology", DisplayOrder = 1 },
                        new() { SourceName = "WAPDA Hydro-telemetry Stream Gauge Records", DisplayOrder = 2 }
                    },
                    MethodologySteps = new List<ProjectMethodologyStep>
                    {
                        new() { StepTitle = "Physical Constraint Loss Function Design", StepDescription = "Formulating residual error penalties for conservation of mass between precipitation, evapotranspiration, and discharge.", DisplayOrder = 1 }
                    },
                    Technologies = new List<ProjectTechnology>
                    {
                        new() { TechnologyName = "PyTorch & Lightning Framework", DisplayOrder = 1 },
                        new() { TechnologyName = "Python Xarray & Dask Multi-threading", DisplayOrder = 2 }
                    },
                    KeyFindings = new List<ProjectKeyFinding>
                    {
                        new() { Finding = "Achieved 0.88 NSE on 72-hour lead time flood peak hydrograph forecasts across testing sub-basins.", DisplayOrder = 1 }
                    },
                    ProjectResearchAreas = new List<ProjectResearchArea>
                    {
                        new() { ResearchDomainId = aiDomain.Id }
                    }
                };

                context.Projects.AddRange(p1, p2, p3, p4);
                await context.SaveChangesAsync();
            }

            // 7. Seed Publications
            if (!await context.Publications.AnyAsync())
            {
                var pub1 = new Publication
                {
                    Title = "Operational Flood Inundation Delineation in the Lower Indus Basin Using Sentinel-1 Synthetic Aperture Radar and Hydrodynamic Level Inversion",
                    Journal = "Journal of Hydrology",
                    Year = 2025,
                    Doi = "10.1016/j.jhydrol.2025.131050",
                    Url = "https://doi.org/10.1016/j.jhydrol.2025.131050",
                    PublicationType = PublicationType.Journal,
                    Abstract = "Monsoon floods across the transboundary Indus River Basin cause devastating socio-economic damage. This paper presents an end-to-end operational framework integrating Google Earth Engine C-band Sentinel-1 SAR observations with calibrated 2D hydraulic terrain models to provide rapid-response water extent and depth estimations across flooded agricultural districts.",
                    Featured = true,
                    CitationMetrics = "Impact Factor: 6.4 | Citations: 12",
                    Authors = new List<PublicationAuthor>
                    {
                        new() { AuthorName = "Boota, M. W.", IsLabMember = true, DisplayOrder = 1 },
                        new() { AuthorName = "Tahir, U. A.", IsLabMember = true, DisplayOrder = 2 },
                        new() { AuthorName = "Zahra, F.", IsLabMember = true, DisplayOrder = 3 }
                    },
                    PublicationResearchAreas = new List<PublicationResearchArea>
                    {
                        new() { ResearchDomainId = satDomain.Id },
                        new() { ResearchDomainId = floodDomain.Id }
                    }
                };

                var pub2 = new Publication
                {
                    Title = "Decadal Groundwater Depletion and Aquifer Depletion Acceleration in the Indus Basin Irrigation System Revealed by GRACE/GRACE-FO Gravimetry",
                    Journal = "Water Resources Research",
                    Year = 2024,
                    Doi = "10.1029/2023WR035210",
                    Url = "https://doi.org/10.1029/2023WR035210",
                    PublicationType = PublicationType.Journal,
                    Abstract = "Unregulated groundwater pumping for intensified double-cropping has stressed the Indus Alluvial Aquifer. By isolating groundwater mass anomalies from GRACE/GRACE-FO mascons with regional Land Surface Models, we quantify spatial depletion trajectories from 2002 to 2024 across Pakistan's agricultural breadbasket.",
                    Featured = true,
                    CitationMetrics = "Impact Factor: 5.4 | Citations: 28",
                    Authors = new List<PublicationAuthor>
                    {
                        new() { AuthorName = "Boota, M. W.", IsLabMember = true, DisplayOrder = 1 },
                        new() { AuthorName = "Noor, Z.", IsLabMember = true, DisplayOrder = 2 },
                        new() { AuthorName = "Miller, S.", IsLabMember = false, DisplayOrder = 3 }
                    },
                    PublicationResearchAreas = new List<PublicationResearchArea>
                    {
                        new() { ResearchDomainId = gwDomain.Id },
                        new() { ResearchDomainId = satDomain.Id }
                    }
                };

                var pub3 = new Publication
                {
                    Title = "Physics-Informed Neural Networks for High-Lead Streamflow Forecasting in Alpine Catchments with Snowmelt Dynamics",
                    Journal = "Remote Sensing of Environment",
                    Year = 2024,
                    Doi = "10.1016/j.rse.2024.114002",
                    Url = "https://doi.org/10.1016/j.rse.2024.114002",
                    PublicationType = PublicationType.Journal,
                    Abstract = "Accurate streamflow forecasting in high-relief snow-fed basins requires balancing complex machine learning representation with strict mass conservation physics. We introduce a PINN architecture constrained by satellite snow cover and degree-day ablation equations.",
                    Featured = true,
                    CitationMetrics = "Impact Factor: 13.5 | Citations: 45",
                    Authors = new List<PublicationAuthor>
                    {
                        new() { AuthorName = "Tahir, U. A.", IsLabMember = true, DisplayOrder = 1 },
                        new() { AuthorName = "Boota, M. W.", IsLabMember = true, DisplayOrder = 2 }
                    },
                    PublicationResearchAreas = new List<PublicationResearchArea>
                    {
                        new() { ResearchDomainId = aiDomain.Id },
                        new() { ResearchDomainId = cryoDomain.Id }
                    }
                };

                var pub4 = new Publication
                {
                    Title = "Glacial Lake Evolution and Outburst Flood Hazard Zoning in the Karakoram Range Under Accelerated Warming",
                    Journal = "Geomorphology",
                    Year = 2023,
                    Doi = "10.1016/j.geomorph.2023.108740",
                    Url = "https://doi.org/10.1016/j.geomorph.2023.108740",
                    PublicationType = PublicationType.Journal,
                    Abstract = "Comprehensive inventory of high-risk glacial lakes across the Hunza and Shigar valleys utilizing Sentinel-2 and declassified historical Corona satellite imagery.",
                    Featured = false,
                    CitationMetrics = "Impact Factor: 4.3 | Citations: 31",
                    Authors = new List<PublicationAuthor>
                    {
                        new() { AuthorName = "Khan, B. A.", IsLabMember = true, DisplayOrder = 1 },
                        new() { AuthorName = "Boota, M. W.", IsLabMember = true, DisplayOrder = 2 }
                    },
                    PublicationResearchAreas = new List<PublicationResearchArea>
                    {
                        new() { ResearchDomainId = cryoDomain.Id }
                    }
                };

                var pub5 = new Publication
                {
                    Title = "Near-Real-Time Flash Drought Assessment in South Asia Using Multi-Sensor Evaporative Stress and Soil Moisture Anomaly Indices",
                    Journal = "International Conference on Hydroinformatics (HIC 2024)",
                    Year = 2024,
                    Doi = "10.1109/HIC.2024.998214",
                    Url = "https://ieeexplore.ieee.org",
                    PublicationType = PublicationType.Conference,
                    Abstract = "Presented findings on rapid-onset agricultural flash drought early detection utilizing MODIS thermal infrared evapotranspiration and SMAP L-band soil moisture.",
                    Featured = false,
                    CitationMetrics = "Peer-Reviewed Conference",
                    Authors = new List<PublicationAuthor>
                    {
                        new() { AuthorName = "Zahra, F.", IsLabMember = true, DisplayOrder = 1 },
                        new() { AuthorName = "Boota, M. W.", IsLabMember = true, DisplayOrder = 2 }
                    },
                    PublicationResearchAreas = new List<PublicationResearchArea>
                    {
                        new() { ResearchDomainId = floodDomain.Id }
                    }
                };

                var pub6 = new Publication
                {
                    Title = "Technical Assessment Report: Transboundary Indus Basin Hydrometeorological Monitoring and Flood Preparedness",
                    Journal = "CEWRE Technical Report Series No. 2023-04",
                    Year = 2023,
                    PublicationType = PublicationType.Report,
                    Abstract = "A comprehensive technical guidance report prepared for provincial and federal water authorities analyzing telemetry gaps and satellite sensing integration opportunities.",
                    Featured = false,
                    CitationMetrics = "Technical Advisory Report",
                    Authors = new List<PublicationAuthor>
                    {
                        new() { AuthorName = "Boota, M. W.", IsLabMember = true, DisplayOrder = 1 }
                    },
                    PublicationResearchAreas = new List<PublicationResearchArea>
                    {
                        new() { ResearchDomainId = satDomain.Id },
                        new() { ResearchDomainId = floodDomain.Id }
                    }
                };

                context.Publications.AddRange(pub1, pub2, pub3, pub4, pub5, pub6);
                await context.SaveChangesAsync();
            }

            // 8. Seed Events
            if (!await context.Events.AnyAsync())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Slug = "seminar-spaceborne-radar-flood-hydrology-2026",
                        Title = "National Seminar: Spaceborne SAR and SWOT Altimetry for Basin-Scale Hydrology",
                        EventType = EventType.Seminar,
                        EventDate = DateTime.UtcNow.AddDays(15),
                        EndDate = DateTime.UtcNow.AddDays(15).AddHours(4),
                        Location = "Auditorium, Centre of Excellence in Water Resources Engineering (CEWRE), UET Lahore",
                        Description = "Join HSRL researchers and guest scientists from national water agencies as we discuss operational applications of Sentinel-1 Synthetic Aperture Radar and SWOT satellite altimetry in managing extreme floods across Pakistan.",
                        Speaker = "Prof. Dr. Muhammad Waseem Boota & Guest Lecturers",
                        Affiliation = "CEWRE, UET Lahore & International Water Management Institute (IWMI)",
                        Status = EventStatus.Upcoming,
                        Featured = true,
                        LinkUrl = "/collaborate"
                    },
                    new Event
                    {
                        Slug = "workshop-google-earth-engine-water-analytics",
                        Title = "Hands-On Workshop: Cloud-Native Earth Observation with Google Earth Engine for Water Resources",
                        EventType = EventType.Workshop,
                        EventDate = DateTime.UtcNow.AddDays(35),
                        EndDate = DateTime.UtcNow.AddDays(36),
                        Location = "Computer Laboratory 3, CEWRE, UET Lahore",
                        Description = "An intensive 2-day technical workshop for graduate researchers and water engineering professionals on writing cloud scripts for satellite precipitation downscaling, flood masking, and evapotranspiration calculations.",
                        Speaker = "Dr. Usman Ali Tahir & Engr. Fatima Zahra",
                        Affiliation = "HSRL, CEWRE UET Lahore",
                        Status = EventStatus.Upcoming,
                        Featured = true,
                        LinkUrl = "/opportunities"
                    },
                    new Event
                    {
                        Slug = "fieldwork-expedition-upper-indus-glaciers-2025",
                        Title = "Upper Indus Basin Glacio-Hydrological Fieldwork & In-Situ Instrumentation Campaign",
                        EventType = EventType.Fieldwork,
                        EventDate = DateTime.UtcNow.AddMonths(-3),
                        EndDate = DateTime.UtcNow.AddMonths(-3).AddDays(12),
                        Location = "Hunza Valley & Passu Glacier, Gilgit-Baltistan, Pakistan",
                        Description = "HSRL field research expedition conducting high-altitude GPS surveys, drone multispectral ablation mapping, and in-situ hydrometric stream velocity measurements near glacier terminus lakes.",
                        Speaker = "HSRL Cryosphere Research Team",
                        Affiliation = "CEWRE, UET Lahore",
                        Status = EventStatus.Past,
                        Featured = false
                    },
                    new Event
                    {
                        Slug = "webinar-ai-physics-informed-hydrology",
                        Title = "International Webinar: Bridging Numerical Hydraulics and Deep Learning",
                        EventType = EventType.Webinar,
                        EventDate = DateTime.UtcNow.AddMonths(-6),
                        Location = "Online (Virtual Broadcast)",
                        Description = "Panel discussion featuring international speakers exploring how Physics-Informed Neural Networks improve flood and drought risk forecasting in data-sparse river basins.",
                        Speaker = "Dr. Usman Ali Tahir & Prof. Dr. Stephen Miller (ETH Zurich)",
                        Affiliation = "HSRL & ETH Zurich",
                        Status = EventStatus.Past,
                        Featured = false
                    }
                );
                await context.SaveChangesAsync();
            }

            // 9. Seed Gallery Items
            if (!await context.GalleryItems.AnyAsync())
            {
                context.GalleryItems.AddRange(
                    new GalleryItem
                    {
                        Title = "High-Altitude Glacio-Hydrometric Gauging at Passu Glacier",
                        ImageUrl = "/images/gallery/fieldwork-passu.jpg",
                        AltText = "Research team conducting hydrometric discharge measurement at Passu Glacier terminus stream",
                        Caption = "HSRL research team deploying acoustic Doppler flow velocity instrumentation in high-altitude Gilgit-Baltistan.",
                        Category = GalleryCategory.Fieldwork,
                        Location = "Passu Glacier, Hunza Valley, Gilgit-Baltistan",
                        EventDate = DateTime.UtcNow.AddMonths(-3),
                        DisplayOrder = 1
                    },
                    new GalleryItem
                    {
                        Title = "Satellite Radar Flood Extent Delineation of 2022 Mega-Flood",
                        ImageUrl = "/images/gallery/sar-flood-map.jpg",
                        AltText = "Multi-temporal satellite radar map showing inundation extents along the Lower Indus River",
                        Caption = "High-resolution Sentinel-1 SAR classification map highlighting inundated agricultural districts in Sindh.",
                        Category = GalleryCategory.Research,
                        Location = "Lower Indus Plain, Sindh",
                        EventDate = DateTime.UtcNow.AddMonths(-18),
                        DisplayOrder = 2
                    },
                    new GalleryItem
                    {
                        Title = "CEWRE Remote Sensing & GIS Computing Laboratory",
                        ImageUrl = "/images/gallery/lab-workstation.jpg",
                        AltText = "Graduate students analyzing satellite imagery on dual-monitor workstations in HSRL",
                        Caption = "Postgraduate research scholars processing planetary-scale satellite archives in the HSRL computer cluster.",
                        Category = GalleryCategory.Workshops,
                        Location = "CEWRE, UET Lahore",
                        EventDate = DateTime.UtcNow.AddMonths(-2),
                        DisplayOrder = 3
                    },
                    new GalleryItem
                    {
                        Title = "Technical Consultation Session with Provincial Irrigation Engineers",
                        ImageUrl = "/images/gallery/stakeholder-workshop.jpg",
                        AltText = "Lab director presenting satellite water storage maps to irrigation and drainage engineers",
                        Caption = "Prof. Dr. Muhammad Waseem presenting GRACE aquifer depletion findings to government engineers.",
                        Category = GalleryCategory.Outreach,
                        Location = "Lahore, Pakistan",
                        EventDate = DateTime.UtcNow.AddMonths(-5),
                        DisplayOrder = 4
                    },
                    new GalleryItem
                    {
                        Title = "Drone Multispectral Aerial Survey of River Morphodynamics",
                        ImageUrl = "/images/gallery/drone-survey.jpg",
                        AltText = "Drone hovering over river reach collecting high-resolution multispectral imagery",
                        Caption = "Reach-scale aerial drone photogrammetry for riverbank erosion and vegetation roughness characterization.",
                        Category = GalleryCategory.Fieldwork,
                        Location = "Ravi River reach near Lahore",
                        EventDate = DateTime.UtcNow.AddMonths(-8),
                        DisplayOrder = 5
                    }
                );
                await context.SaveChangesAsync();
            }

            // 10. Seed Opportunities & Categories
            if (!await context.OpportunityCategories.AnyAsync())
            {
                context.OpportunityCategories.AddRange(
                    new OpportunityCategory
                    {
                        Title = "PhD Candidacy & Doctoral Fellowships",
                        ShortDescription = "Funded and research-linked doctoral programs through CEWRE, UET Lahore, in advanced satellite hydrology and hydroclimatic AI.",
                        EngagementType = OpportunityEngagementType.Phd,
                        DisplayOrder = 1,
                        RecurringPathways = new List<OpportunityRecurringPathway>
                        {
                            new() { Pathway = "Admissions open bi-annually (Fall and Spring semesters) through CEWRE UET Lahore.", DisplayOrder = 1 },
                            new() { Pathway = "Eligible candidates possess an MS/MPhil in Water Resources, Hydrology, Remote Sensing, or Civil Engineering.", DisplayOrder = 2 },
                            new() { Pathway = "Direct alignment with sponsored international and national research grants.", DisplayOrder = 3 }
                        }
                    },
                    new OpportunityCategory
                    {
                        Title = "MS / MPhil Research Positions",
                        ShortDescription = "Rigorous postgraduate research opportunities exploring satellite data processing, GIS, and hydrological modeling.",
                        EngagementType = OpportunityEngagementType.MsMphil,
                        DisplayOrder = 2,
                        RecurringPathways = new List<OpportunityRecurringPathway>
                        {
                            new() { Pathway = "Coursework combined with thesis research supervised by HSRL faculty.", DisplayOrder = 1 },
                            new() { Pathway = "Access to high-performance GPU clusters, cloud accounts, and field instrumentation.", DisplayOrder = 2 }
                        }
                    },
                    new OpportunityCategory
                    {
                        Title = "Research Associate (RA) & Technical Staff",
                        ShortDescription = "Full-time project-funded research associate appointments on funded research projects.",
                        EngagementType = OpportunityEngagementType.Ra,
                        DisplayOrder = 3,
                        RecurringPathways = new List<OpportunityRecurringPathway>
                        {
                            new() { Pathway = "Competitive monthly stipends aligned with HEC and donor grant regulations.", DisplayOrder = 1 },
                            new() { Pathway = "Authorship opportunities on peer-reviewed international journal manuscripts.", DisplayOrder = 2 }
                        }
                    },
                    new OpportunityCategory
                    {
                        Title = "Undergraduate & Graduate Summer Internships",
                        ShortDescription = "Hands-on 8-12 week summer training in remote sensing imagery, python GIS, and hydrologic tools.",
                        EngagementType = OpportunityEngagementType.Internship,
                        DisplayOrder = 4,
                        RecurringPathways = new List<OpportunityRecurringPathway>
                        {
                            new() { Pathway = "Open to university students in Civil, Environmental, Computer Science, and Geomatics programs.", DisplayOrder = 1 },
                            new() { Pathway = "Formal certificate of completion issued by HSRL and CEWRE UET Lahore.", DisplayOrder = 2 }
                        }
                    }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.OfficialOpportunities.AnyAsync())
            {
                context.OfficialOpportunities.AddRange(
                    new OfficialOpportunity
                    {
                        Title = "Doctoral Research Fellowship: Physics-Informed Deep Learning for Indus Basin Streamflow",
                        Category = "PhD Position",
                        Deadline = DateTime.UtcNow.AddDays(45),
                        Description = "Full-time doctoral fellowship investigating the integration of satellite remote sensing observations with physics-informed deep neural networks to improve multi-day river discharge forecasting across the Indus Basin.",
                        Status = OpportunityStatus.Open,
                        Link = "/collaborate",
                        DisplayOrder = 1
                    },
                    new OfficialOpportunity
                    {
                        Title = "Research Associate: Satellite Radar Altimetry & Inundation Analysis",
                        Category = "Research Associate",
                        Deadline = DateTime.UtcNow.AddDays(25),
                        Description = "Seeking a motivated researcher with strong Python, GDAL, and satellite radar (SAR) background to support our near-real-time flood mapping and altimetry inversion project.",
                        Status = OpportunityStatus.Open,
                        Link = "/collaborate",
                        DisplayOrder = 2
                    },
                    new OfficialOpportunity
                    {
                        Title = "Graduate Research Internship: Cloud-Native Earth Observation for Soil Moisture",
                        Category = "Internship",
                        Deadline = DateTime.UtcNow.AddDays(60),
                        Description = "3-month summer internship for senior undergraduate or beginning master's students focusing on Google Earth Engine pipelines for SMAP and Sentinel-1 data fusion.",
                        Status = OpportunityStatus.Open,
                        Link = "/collaborate",
                        DisplayOrder = 3
                    }
                );
                await context.SaveChangesAsync();
            }

            // 11. Seed Collaboration Sectors
            if (!await context.CollaborationSectors.AnyAsync())
            {
                context.CollaborationSectors.AddRange(
                    new CollaborationSector
                    {
                        Title = "Academic & Research Institutions",
                        Category = CollaborationCategory.Academia,
                        Description = "We actively partner with national and international universities and research institutes on joint grant proposals, co-supervision of graduate theses, faculty exchanges, and high-impact scientific publications.",
                        IconName = "bi-mortarboard",
                        DisplayOrder = 1,
                        EngagementAvenues = new List<CollaborationEngagementAvenue>
                        {
                            new() { Avenue = "Joint research proposals for HEC, UNESCO, Horizon Europe, and international grant calls", DisplayOrder = 1 },
                            new() { Avenue = "Co-advising PhD scholars and hosting visiting research fellows", DisplayOrder = 2 },
                            new() { Avenue = "Shared hydrometeorological datasets, benchmarks, and model codes", DisplayOrder = 3 }
                        }
                    },
                    new CollaborationSector
                    {
                        Title = "Government & Water Management Authorities",
                        Category = CollaborationCategory.Government,
                        Description = "Providing state-of-the-art technical advisory, satellite earth observation products, and decision-support systems to national and provincial water authorities.",
                        IconName = "bi-building-check",
                        DisplayOrder = 2,
                        EngagementAvenues = new List<CollaborationEngagementAvenue>
                        {
                            new() { Avenue = "Operational flood hazard zoning and damage assessment technical assistance", DisplayOrder = 1 },
                            new() { Avenue = "Aquifer depletion monitoring and sustainable groundwater regulatory frameworks", DisplayOrder = 2 },
                            new() { Avenue = "Customized capacity-building workshops for water and disaster management engineers", DisplayOrder = 3 }
                        }
                    },
                    new CollaborationSector
                    {
                        Title = "International Development Agencies & NGOs",
                        Category = CollaborationCategory.Ngo,
                        Description = "Collaborating with multilateral organizations (World Bank, ADB, ICIMOD, IWMI) on climate adaptation studies, disaster risk reduction, and transboundary water governance.",
                        IconName = "bi-globe",
                        DisplayOrder = 3,
                        EngagementAvenues = new List<CollaborationEngagementAvenue>
                        {
                            new() { Avenue = "Climate vulnerability and adaptation diagnostic studies", DisplayOrder = 1 },
                            new() { Avenue = "Independent scientific reviews and environmental impact evaluations", DisplayOrder = 2 }
                        }
                    },
                    new CollaborationSector
                    {
                        Title = "Industry & Consulting Engineering Firms",
                        Category = CollaborationCategory.Industry,
                        Description = "Offering specialized numerical modeling, hydrodynamic simulation, and satellite remote sensing consultancy for infrastructure planning, dam safety, and irrigation rehabilitation.",
                        IconName = "bi-briefcase",
                        DisplayOrder = 4,
                        EngagementAvenues = new List<CollaborationEngagementAvenue>
                        {
                            new() { Avenue = "Contracted 2D hydraulic flood propagation and dam-break simulations", DisplayOrder = 1 },
                            new() { Avenue = "High-resolution geospatial data processing and watershed delineation services", DisplayOrder = 2 }
                        }
                    }
                );
                await context.SaveChangesAsync();
            }

            // 12. Seed Sample Inquiries (for admin review testing)
            if (!await context.CollaborationInquiries.AnyAsync())
            {
                context.CollaborationInquiries.AddRange(
                    new CollaborationInquiry
                    {
                        FullName = "Dr. Tariq Mahmood",
                        Email = "tariq.mahmood@watercouncil.org.pk",
                        Organization = "Pakistan Water Resource Council",
                        AffiliationType = AffiliationType.Government,
                        CollaborationType = CollaborationType.AppliedResearch,
                        ResearchDomainId = floodDomain.Id,
                        Title = "Partnership on Real-Time Flood Warning System for Upper Chenab",
                        Description = "We are seeking technical collaboration with HSRL to pilot your Sentinel-1 flood depth inversion model in the Upper Chenab irrigation zone. We would like to schedule a coordination meeting with Prof. Dr. Muhammad Waseem.",
                        SubmittedAtUtc = DateTime.UtcNow.AddDays(-2),
                        Status = InquiryStatus.New,
                        IsSpam = false
                    },
                    new CollaborationInquiry
                    {
                        FullName = "Engr. Ayesha Siddiqui",
                        Email = "ayesha.siddiqui@nust.edu.pk",
                        Organization = "National University of Sciences & Technology (NUST)",
                        AffiliationType = AffiliationType.Academic,
                        CollaborationType = CollaborationType.ResearchCollaboration,
                        ResearchDomainId = aiDomain.Id,
                        Title = "Joint HEC National Technology Fund Research Proposal",
                        Description = "Our lab at NUST is preparing an HEC research grant proposal on AI downscaling of climate models for mountain water catchments. We would like to explore HSRL as a co-investigating partner.",
                        SubmittedAtUtc = DateTime.UtcNow.AddDays(-5),
                        Status = InquiryStatus.Reviewed,
                        IsSpam = false,
                        AdminNotes = "Reviewed by Director. Positive match for ongoing PINN project."
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
