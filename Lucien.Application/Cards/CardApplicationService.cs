using AutoMapper;
using Lucien.Application.Contracts.Cards.Dto;
using Lucien.Application.Contracts.Cards.Interfaces;
using Lucien.Domain.Cards.Entities;
using Lucien.Domain.Cards.Interfaces;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using Lucien.Application.Contracts.Common.Dto;

namespace Lucien.Application.Cards
{
    public class CardApplicationService : ICardApplicationService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardApplicationService(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public  Task<List<CardDto>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CardDto> CreateAsync(CreateCardDto input)
        {
            var card = _mapper.Map<Card>(input);
            var createdCard = await _cardRepository.CreateAsync(card);
            return _mapper.Map<CardDto>(createdCard);
        }

        public async Task<CardDto> UpdateAsync(Guid id, UpdateCardDto input)
        {
            var card = _mapper.Map<Card>(input);
            var updatedCard = await _cardRepository.UpdateAsync(id, card);
            return _mapper.Map<CardDto>(updatedCard);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _cardRepository.DeleteAsync(id);
        }

        public async Task<FileResultDto> ExportAsync(string format)
        {
            var cards = new List<Card>();
            var cardsDto = _mapper.Map<List<CardDto>>(cards);
            if (format == "xml")
            {
                var xmlSerializer = new XmlSerializer(typeof(List<CardDto>));
                using var stringWriter = new StringWriter();
                xmlSerializer.Serialize(stringWriter, cardsDto);

                var xmlContent = Encoding.UTF8.GetBytes(stringWriter.ToString());
                return await Task.FromResult(new FileResultDto(xmlContent, "application/xml", "cards.xml"));
            }
            else
            {
                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream);
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.WriteRecords(cardsDto);
                streamWriter.Flush();

                var csvContent = memoryStream.ToArray();
                return await Task.FromResult(new FileResultDto(csvContent, "text/csv", "cards.csv"));
            }
        }

        public async Task<CardDto> GetAsync(Guid id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            return _mapper.Map<CardDto>(card);
        }

        public CardDto Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            return extension switch
            {
                ".csv" => ImportFromCsv(file),
                ".xml" => ImportFromXml(file),
                _ => throw new NotSupportedException("File type not supported."),
            };
        }

        private CardDto ImportFromCsv(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CardDto>().ToList();

            return records.FirstOrDefault() ?? throw new InvalidOperationException("No records found in CSV.");
        }

        private CardDto ImportFromXml(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var serializer = new XmlSerializer(typeof(CardDto));

            var record = (CardDto?)serializer.Deserialize(stream);

            return record ?? throw new InvalidOperationException("No records found in XML.");
        }

        public Task<CardDto> ImportFromQrCodeAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }


    }
}
