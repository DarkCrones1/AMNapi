// using System.Linq.Expressions;
// using System.Net;

// using Microsoft.AspNetCore.Mvc;

// using AutoMapper;
// using Microsoft.EntityFrameworkCore;

// using AMNApi.Data;
// using AMNApi.Dtos.Response;
// using AMNApi.Dtos.Request.Create;
// using AMNApi.Entities;
// // using AMNApi.Dtos.QueryFilters;
// using AMNApi.Filters.Exceptions;
// using AMNApi.Entities.Base;
// using Microsoft.AspNetCore.Authorization;
// using AMNApi.Response;
// using AMNApi.Common.Functions;
// using AMNApi.Dtos.QueryFilters;

// namespace AMNApi.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// [Produces("application/json")]
// public class MapLocationController : ControllerBase
// {
//     private readonly IMapper _mapper;
//     private readonly AMNDbContext _dbContext;

//     public MapLocationController(IMapper mapper, AMNDbContext dbContext)
//     {
//         this._mapper = mapper;
//         this._dbContext = dbContext;
//     }

//     [HttpGet]
//     [Route("")]
//     [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<MapLocationResponseDto>>))]
//     [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<MapLocationResponseDto>>))]
//     [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IEnumerable<MapLocationResponseDto>>))]
//     public async Task<IActionResult> GetAll([FromQuery] MapLocationQueryFilter filter)
//     {
//         try
//         {
//             var entities = await GetPageds(filter);
//             var dtos = _mapper.Map<IEnumerable<MapLocationResponseDto>>(entities);
//             var metaDataResponse = new MetaDataResponse(
//                 entities.TotalCount,
//                 entities.CurrentPage,
//                 entities.PageSize
//             );
//             var response = new ApiResponse<IEnumerable<MapLocationResponseDto>>(data: dtos, meta: metaDataResponse);
//             return Ok(response);
//         }
//         catch (Exception ex)
//         {
//             throw new LogicBusinessException(ex);
//         }
//     }

//     [HttpPost]
//     [Route("")]
//     [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<MapLocationResponseDto>))]
//     public async Task<IActionResult> Create([FromBody] MapLocationCreateRequestDto requestDto)
//     {
//         try
//         {
//             Expression<Func<Consultory, bool>> Consultory = x => x.Id == requestDto.ConsultoryId;

//             var existConsultory = await _dbContext.Consultory.AnyAsync(Consultory);

//             if (!existConsultory)
//                 return BadRequest("no existe el consultorio");

//             Expression<Func<MapLocation, bool>> filterConsultory = x => !x.IsDeleted!.Value && x.ConsultoryId == requestDto.ConsultoryId && x.Consultory.MapLocation.Any();

//             var mapConsultory = await _dbContext.MapLocation.AnyAsync(filterConsultory);

//             if (mapConsultory)
//                 return BadRequest("Un consultorio no puede tener mas de 2 direcciones");

//             var entity = _mapper.Map<MapLocation>(requestDto);
//             await _dbContext.MapLocation.AddAsync(entity);
//             await _dbContext.SaveChangesAsync();

//             var result = _mapper.Map<MapLocationResponseDto>(entity);
//             var response = new ApiResponse<MapLocationResponseDto>(result);
//             return Ok(response);
//         }
//         catch (Exception ex)
//         {

//             throw new LogicBusinessException(ex);
//         }
//     }

//     private async Task<PagedList<MapLocation>> GetPageds(MapLocationQueryFilter filter)
//     {
//         var result = await GetPaged(filter);
//         var pagedItems = PagedList<MapLocation>.Create(result, filter.PageNumber, filter.PageSize);
//         return pagedItems;
//     }

//     private async Task<IEnumerable<MapLocation>> GetPaged(MapLocationQueryFilter entity)
//     {
//         var query = _dbContext.MapLocation.AsQueryable();

//         if (entity.Id > 0)
//             query = query.Where(x => x.Id == entity.Id);

//         if (entity.ConsultoryId > 0)
//             query = query.Where(x => x.ConsultoryId == entity.ConsultoryId);

//         if (entity.Latitude.HasValue)
//             query = query.Where(x => x.Latitude == entity.Latitude);

//         if (entity.Longitude.HasValue)
//             query = query.Where(x => x.Longitude == entity.Longitude);

//         return await query.ToListAsync();
//     }
// }