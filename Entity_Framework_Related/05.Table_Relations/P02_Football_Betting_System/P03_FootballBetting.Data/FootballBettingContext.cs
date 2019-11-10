

namespace P03_FootballBetting.Data
{

    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FootballBettingContext : DbContext
    {

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {

            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Settings.ConnectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Team>(t =>
            {
                t.HasKey(x => x.TeamId);

                t.HasOne(x => x.PrimaryKitColor)
                .WithMany(pk => pk.PrimaryKitTeams)
                .HasForeignKey(x => x.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.SecondaryKitColor)
                .WithMany(x => x.SecondaryKitTeams)
                .HasForeignKey(x => x.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.Town)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.TownId)
                .OnDelete(DeleteBehavior.Restrict);

                //here maybe mistake? TODO: 
            });

            modelBuilder.Entity<Town>(t =>
            {
                t.HasKey(z => z.TownId);

                t.HasOne(x => x.Country)
                .WithMany(x => x.Towns)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Country>(c =>
            {
                c.HasKey(x => x.CountryId);
            });

            modelBuilder.Entity<Player>(p =>
            {
                p.HasKey(x => x.PlayerId);

                p.HasOne(x => x.Position)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(x => x.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(x => x.TeamId);

                //the players statistics will be arranged in the connecting table! 

            });

            modelBuilder.Entity<Position>(po =>
            {
                po.HasKey(p => p.PositionId);
            });

            modelBuilder.Entity<Game>(g =>
            {
                g.HasKey(y => y.GameId);

                g.HasOne(x => x.HomeTeam)
                .WithMany(x => x.HomeGames)
                .HasForeignKey(x => x.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                g.HasOne(x => x.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(x => x.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                //TODO: one mistake here => when you start with many DO NOT ADD FOREIGN KEY! 
                g.HasMany(p => p.Bets)
                .WithOne(b => b.Game)
                //.HasForeignKey(g => g.BetId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PlayerStatistic>(ps =>
            {
                ps.HasKey(x => new { x.PlayerId, x.GameId });

                ps.HasOne(m => m.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(z => z.GameId)
                .OnDelete(DeleteBehavior.Restrict);

                ps.HasOne(a => a.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Bet>(b =>
            {
                b.HasKey(x => x.BetId);

                b.
                Property(y => y.Prediction)
                .IsRequired(true);

                b.HasOne(x => x.User)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(o => o.UserId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
