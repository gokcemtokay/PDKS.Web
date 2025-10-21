using AutoMapper;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using System.Linq;

namespace PDKS.Business.Mapping
{
    // AutoMapper'ın dönüşüm kurallarını içeren ana profil sınıfı
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Vardiya -> VardiyaListDTO
            CreateMap<Vardiya, VardiyaListDTO>()
                // SirketId mapping'i eklendi
                .ForMember(dest => dest.SirketId, opt => opt.MapFrom(src => src.SirketId))
                // DTO'daki diğer alanlar otomatik olarak maplenir
                .ReverseMap();

            // Vardiya -> VardiyaDetailDTO
            CreateMap<Vardiya, VardiyaDetailDTO>()
                 .ForMember(dest => dest.SirketId, opt => opt.MapFrom(src => src.SirketId))
                .ReverseMap();

            // VardiyaCreateDTO/VardiyaUpdateDTO -> Vardiya
            CreateMap<VardiyaCreateDTO, Vardiya>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Yeni kayıt olduğu için ID'yi ignore et

            CreateMap<VardiyaUpdateDTO, Vardiya>();


            // ===============================================
            // PERSONEL MAPPINGS (Örnek Projeksiyon)
            // ===============================================

            // Personel -> PersonelListDTO
            CreateMap<Personel, PersonelListDTO>()
                .ForMember(dest => dest.Departman, opt => opt.MapFrom(src => src.Departman != null ? src.Departman.Ad : string.Empty))
                .ForMember(dest => dest.VardiyaAdi, opt => opt.MapFrom(src => src.Vardiya != null ? src.Vardiya.Ad : string.Empty))
                // NOT: Serviste elle projeksiyon yaptığınız için bu mapping'ler sadece genel kullanım içindir.
                .ReverseMap();

            // Personel DTO -> Entity
            CreateMap<PersonelCreateDTO, Personel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<PersonelUpdateDTO, Personel>().ReverseMap();
            CreateMap<PersonelDetailDTO, Personel>().ReverseMap();

            // ===============================================
            // DEPARTMAN MAPPINGS
            // ===============================================
            CreateMap<Departman, DepartmanListDTO>()
                .ForMember(dest => dest.DepartmanAdi, opt => opt.MapFrom(src => src.Ad))
                .ForMember(dest => dest.UstDepartmanAdi, opt => opt.MapFrom(src => src.UstDepartman != null ? src.UstDepartman.Ad : null))
                .ReverseMap();

            CreateMap<Departman, DepartmanDetailDTO>()
                .ForMember(dest => dest.DepartmanAdi, opt => opt.MapFrom(src => src.Ad))
                .ForMember(dest => dest.UstDepartmanAdi, opt => opt.MapFrom(src => src.UstDepartman != null ? src.UstDepartman.Ad : null))
                .ReverseMap();

            CreateMap<DepartmanCreateDTO, Departman>()
                .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.DepartmanAdi))
                .ReverseMap();

            CreateMap<DepartmanUpdateDTO, Departman>()
                .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.DepartmanAdi))
                .ReverseMap();

            // ===============================================
            // DİĞER CORE ENTITY MAPPINGS
            // ===============================================
            CreateMap<Izin, IzinListDTO>().ReverseMap();
            CreateMap<Mesai, MesaiListDTO>().ReverseMap();
            CreateMap<Sirket, SirketListDTO>().ReverseMap();
            CreateMap<Sirket, SirketDetailDTO>().ReverseMap();
            CreateMap<SirketCreateDTO, Sirket>().ReverseMap();
        }
    }
}