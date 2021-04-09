﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models.SQLite;
using SQLite;

namespace NDB.Covid19.PersistedData.SQLite
{
    public interface ILoggingManager
    {
        void SaveNewLog(LogSQLiteModel log);
        Task<List<LogSQLiteModel>> GetLogs(int amount);
        Task DeleteLogs(List<LogSQLiteModel> logs);
        Task DeleteAll();
    }

    public class LoggingSQLiteManager : ILoggingManager
    {
        private static readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);
        private readonly SQLiteAsyncConnection _database;

        public LoggingSQLiteManager()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Conf.DB_NAME);
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LogSQLiteModel>().Wait();
        }

        public async void SaveNewLog(LogSQLiteModel log)
        {
            await _syncLock.WaitAsync();
            try
            {
                await _database.InsertAsync(log);

                Debug.Print($"Logged {log.Severity}: {log.Description}");
                if (log.ExceptionMessage != null)
                {
                    Debug.Print($"({log.ExceptionType}) {log.ExceptionMessage}");
                    Debug.Print(log.ExceptionStackTrace);
                }

                if (log.CorrelationId != null)
                {
                    Debug.Print(log.CorrelationId);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<List<LogSQLiteModel>> GetLogs(int amount)
        {
            await _syncLock.WaitAsync();
            try
            {
                return await _database.Table<LogSQLiteModel>()
                    .Take(amount)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return new List<LogSQLiteModel>();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task DeleteLogs(List<LogSQLiteModel> logs)
        {
            await _syncLock.WaitAsync();
            try
            {
                foreach (LogSQLiteModel log in logs)
                {
                    await _database.Table<LogSQLiteModel>().DeleteAsync(it => it.ID == log.ID);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task DeleteAll()
        {
            await _syncLock.WaitAsync();
            try
            {
                await _database.DeleteAllAsync<LogSQLiteModel>();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
            finally
            {
                _syncLock.Release();
            }
        }
    }
}