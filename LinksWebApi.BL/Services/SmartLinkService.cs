using AutoMapper;
using LinksWebApi.BL.Dto;
using LinksWebApi.BL.Interfaces;
using LinksWebApi.Data.Entities;
using LinksWebApi.Data.Interfaces;

namespace LinksWebApi.BL.Services
{
    public class SmartLinkService : ISmartLinkService
    {
        private readonly ISmartLinkRepository _repository;
        private readonly IMapper _mapper;

        public SmartLinkService(ISmartLinkRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private string NormalizeRelativeUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            if (Uri.TryCreate(relativeUrl, UriKind.Relative, out var result))
            {
                return result.ToString();
            }

            throw new ApplicationException("Не получается нормализовать URL");
        }

        /// <summary>
        /// Создает новый Smart Link и сохраняет его в базе данных.
        /// </summary>
        /// <param name="dto">Данные для создания Smart Link.</param>
        /// <returns>DTO созданного Smart Link.</returns>
        public async Task<SmartLinkDto> Create(SmartLinkBaseDto dto)
        {
            var entity = _mapper.Map<SmartLink>(dto);
            entity.NormalizedOriginRelativeUrl = NormalizeRelativeUrl(entity.OriginRelativeUrl);

            if (await _repository.GetByRelativePathAsync(entity.NormalizedOriginRelativeUrl) != null)
            {
                throw new ApplicationException("Ссылка с таким URL уже существует");
            }

            entity.CreatedOn = DateTimeOffset.Now;
            await _repository.CreateAsync(entity);

            return _mapper.Map<SmartLinkDto>(entity);
        }

        /// <summary>
        /// Получает Smart Link по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link.</param>
        /// <returns>DTO найденного Smart Link или null, если Smart Link не найден.</returns>
        public async Task<SmartLinkDto?> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<SmartLinkDto>(entity);
        }

        /// <summary>
        /// Обновляет существующий Smart Link.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link для обновления.</param>
        /// <param name="dto">Новые данные для URL.</param>
        /// <returns>Обновленный DTO Smart Link.</returns>
        public async Task<SmartLinkDto?> Update(int id, SmartLinkBaseDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OriginRelativeUrl = dto.OriginRelativeUrl;
            entity.NormalizedOriginRelativeUrl = NormalizeRelativeUrl(entity.OriginRelativeUrl);

            await _repository.UpdateAsync(entity);

            return _mapper.Map<SmartLinkDto>(entity);
        }

        /// <summary>
        /// Удаляет Smart Link по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link для удаления.</param>
        /// <returns>True, если Smart Link был удален, иначе false.</returns>
        public Task<bool> Delete(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public async Task<SmartLinkDto?> Find(string relativePath)
        {
            var smartLinkEntity = await _repository.GetByRelativePathAsync(relativePath);
            return smartLinkEntity == null ? null : _mapper.Map<SmartLinkDto>(smartLinkEntity);
        }
    }
}
