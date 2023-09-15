using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        //Get all regions // https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //get data from database - domain models 
            var regionsDomain = await regionRepository.GetAllAsync();

            //return dtos             
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain)); 
        }

        //GET ID by single region  // https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id) 
        {
            //find can only be used for id, not name nor code 
            //var region = dbContext.Regions.Find(id);

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound(); 
            }

            //Map the region domain model to region dto

            return Ok(mapper.Map<RegionDto>(regionDomain));

        }



        //POST To create new region 
        ////Get all regions // https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddRegionRequestDto addRegionRequestDto) 
        {
            //map mor convert dto to domainmodel
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto); 

            //use domain model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain model back to DTO 
            var regionDto = mapper.Map<RegionDto>(regionDomainModel); 

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);

        }

        // update regiion 
        //PUT: / https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]UpdateRegionRequestDto updateRegionRequestDto)
        {
            //mapp dto to domain model 
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto); 
            
            //check if id exist
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel); 

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));

        }

        //Delete Region 
        //Delete: / https://localhost:portnumber/api/regions/{id} 
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();

            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));            
        }

    }
}
