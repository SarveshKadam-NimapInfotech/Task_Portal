﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Data.Repositories.TaskRepo
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync(TaskQueryParameters queryParameters)
        {
            IQueryable<Tasks> query = _context.Tasks;

            if (!string.IsNullOrEmpty(queryParameters.Status))
            {
                query = query.Where(t => t.Status == queryParameters.Status);
            }

            if (queryParameters.DueDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date == queryParameters.DueDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(queryParameters.AssignedTo))
            {
                query = query.Where(t => t.AssignedToUserId == queryParameters.AssignedTo);
            }

            if (queryParameters.SortOrder.ToLower() == "desc")
            {
                query = query.OrderByDescending(t => EF.Property<object>(t, queryParameters.SortBy));
            }
            else
            {
                query = query.OrderBy(t => EF.Property<object>(t, queryParameters.SortBy));
            }

            return await query
                .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();
        }

        public async Task<Tasks> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTaskAsync(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(Tasks task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var taskToDelete = await _context.Tasks.FindAsync(id);
            if (taskToDelete != null)
            {
                _context.Tasks.Remove(taskToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}