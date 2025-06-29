using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain = dbContext.Regions.ToList();

            //Map Domain to DTO if needed
            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });

            }

            return Ok(regionsDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var regionDomain = dbContext.Regions.Find(id);
            // Alternatively, you can use FirstOrDefault if you want to handle cases where the region might not exist
            // var region = dbContext.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map Domain to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);

        }

        [HttpPost]

        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region()
            {
                Name = addRegionRequestDto.Name,
                Code = addRegionRequestDto.Code,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            // Add the new region to the database
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();
            // Map Domain to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }



        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Find the region by ID
            var existingRegion = dbContext.Regions.Find(id);

            if (existingRegion == null)
            {
                return NotFound();
            }

            // Update the region's properties
            existingRegion.Name = updateRegionRequestDto.Name;
            existingRegion.Code = updateRegionRequestDto.Code;
            existingRegion.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            // Save changes to the database
            dbContext.SaveChanges();

            // Map updated domain model to DTO
            var regionDto = new RegionDto()
            {
                Id = existingRegion.Id,
                Name = existingRegion.Name,
                Code = existingRegion.Code,
                RegionImageUrl = existingRegion.RegionImageUrl
            };

            return Ok(regionDto);
        }



        [HttpDelete]
        [Route("{id:Guid}")]

        public IActionResult Delete([FromRoute] Guid id)
        {
            // Find the region by ID
            var regionDomainModel = dbContext.Regions.Find(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Remove the region from the database
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();
           
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }


    }
}
