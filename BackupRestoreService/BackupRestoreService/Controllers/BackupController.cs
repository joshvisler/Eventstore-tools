﻿using BackupRestoreService.Core.Entities;
using BackupRestoreService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackupRestoreService.Controllers
{
    [Route("api/v1/[controller]")]
    public class BackupController : Controller
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpGet]
        public async Task<IEnumerable<Backup>> Get()
        {
            return await _backupService.GetAllBackupsAsync();
        }

        [HttpPost]
        public ActionResult Post([FromBody]Guid clientId)
        {
            if(clientId == Guid.Empty)
            {
                return BadRequest("Client not found");
            }

            return Ok(_backupService.CreateAsync(clientId));
        }

        // DELETE api/values/5
        [HttpDelete]
        public  ActionResult Delete([FromBody]InputParametersBase input)
        {
            if(input.ClientId == Guid.Empty || input.ClientId == null)
            {
                
                return new BadRequestResult();
            }

            return Ok(_backupService.DeleteAsync(input.BackupId, input.ClientId));
        }
    }
}
