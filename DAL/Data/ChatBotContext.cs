using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
	public class ChatBotContext : DbContext
	{
		public ChatBotContext(DbContextOptions<ChatBotContext> options) : base(options) { }

		public DbSet<ChatBot> Bots { get; set; }
	}
}
