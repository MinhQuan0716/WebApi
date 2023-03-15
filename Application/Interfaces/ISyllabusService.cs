﻿﻿using Application.ViewModels.SyllabusModels;
using Application.ViewModels.SyllabusModels.FixViewSyllabus;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using Application.ViewModels.SyllabusModels.ViewDetail;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{

    public interface ISyllabusService { 
     
        public Task<Syllabus> AddSyllabusAsync(SyllabusGeneralDTO syllabus);
        public Task<List<Syllabus>> FilterSyllabus(double duration1, double duration2);
        public Task<List<SyllabusViewAllDTO>> GetAllSyllabus();
        public Task<bool> DeleteSyllabussAsync(string syllabusID);
        public Task<List<Syllabus>> GetByName(string name);
        public Task<bool> UpdateSyllabus(Guid syllabusId, UpdateSyllabusDTO updateItem);

        public Task<SyllabusShowDetailDTO> ViewDetailSyllabus(Guid SyllabusID);

        public Task<FinalViewSyllabusDTO> FinalViewSyllabusDTO(Guid SyllabusID);



    }
}

