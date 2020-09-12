using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class adding4StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp_GetEmployeeAvailableTime = @"CREATE PROCEDURE [dbo].[GetEmployeeAvailableTime] @EmployeeId INT
						, @ServiceDate DATE
						,@ServiceEstimatedTime SMALLINT
					AS
					BEGIN
						SET NOCOUNT ON;

								DECLARE @AvailableTimes TABLE(
									row TINYINT
									, TheTime TIME(7)
									)

						/*
					SELECT TheDate, TheYear, TheMonth, TheDay, TheDayOfWeek, TheTime, StartTime, EndTime, BreakStartTime, BreakEndTime, [EmployeeSchedules].EmployeeId
							*/

							declare @dateLithuanian datetime = CONVERT(datetime, SWITCHOFFSET(GETUTCDATE(), DATEPART(TZOFFSET,
									GETUTCDATE() AT TIME ZONE 'E. Europe Standard Time'))) /*-- in Azure time is saved in UTC so getdate() isn't my timezone so I set in here */

						INSERT INTO @AvailableTimes(
							row
							, TheTime
							)
						SELECT ROW_NUMBER() OVER(
								ORDER BY TheTime
								) AS row
							, TheTime
						FROM[dbo].[DateTable]
						CROSS JOIN[dbo].[TimeTable]
								INNER JOIN[dbo].[EmployeeSchedules] ON[EmployeeSchedules].Day = [DateTable].TheDayOfWeek
						WHERE[DateTable].TheDate = @ServiceDate AND([TimeTable].TheTime BETWEEN StartTime AND EndTime) /*-- laikai darbo laiku */
							AND[TimeTable].TheTime NOT BETWEEN isnull(BreakStartTime, '00:00:00.0000000') AND CASE WHEN(BreakEndTime IS NULL) OR(BreakEndTime = '00:00:00.0000000') THEN '00:00:00.0000000' ELSE cast(dateadd(second, -1, BreakEndTime) AS TIME) END /*-- neiskaitomi laikai, kuriu metu yra pertrauka */
						/* -- sukuriam tai dienai visus galimus laikus, kuriu metu darbuotojas gali atlikti paslauga */
						AND[EmployeeSchedules].EmployeeId = @EmployeeId AND NOT EXISTS(
						   SELECT 1

						   FROM[dbo].[Reservations]

						   WHERE[Reservations].EmployeeId = @EmployeeId AND @ServiceDate = cast([Reservations].[Start] AS DATE) AND([TimeTable].TheTime BETWEEN cast([Reservations].[Start] AS TIME) AND cast(dateadd(second, -1, [Reservations].[End]) AS TIME)) AND[Reservations].StatusId <> 4
								) /*-- tikrinam, kad musu diena tuo laiku butu laisvas laikas is neatsauktu laiku (id=4 yra atsaukta) */
								AND IIF(@ServiceDate = cast(@dateLithuanian AS DATE) AND[TheTime] <= cast(dateadd(minute, 15, @dateLithuanian) AS TIME), 0, 1) = 1--tikrina, kad dabartines dienos atgalinio laiko nerodytu
								AND @ServiceDate >= cast(@dateLithuanian AS DATE)

						DECLARE @ModifiedTimes TABLE(
							row TINYINT
							, TheTime TIME(7)
							, NextAvailableTime TIME(7)
							, MinuteDiff INT
							)

						INSERT INTO @ModifiedTimes
						SELECT row
							,TheTime
							,lead(TheTime) OVER(
								ORDER BY row
								) NextAvailableTime
							,datediff(minute, TheTime, lead(TheTime) OVER(
									ORDER BY row
									)) MinuteDiff
						FROM @AvailableTimes

						DECLARE @CurrentRow INT = (
								SELECT min(row)
								FROM @ModifiedTimes
								)
						DECLARE @MaxRow INT = (
								SELECT max(row)
								FROM @ModifiedTimes
								)
						DECLARE @NextRow INT
						DECLARE @minutes INT
						DECLARE @AvailableTimesWithTheirMinutes TABLE(
							TheTime TIME(7)
							,[Minutes] SMALLINT
							)
							/*
						--kai šią dalį pasiekam, DataTable ir TimeTable lentelių pagalba ir atlikus filtrus, jau turim visus laisvus laikus pateiktą dieną, tad 
						--turėdami ir laiką, kiek truks paslaugos, randame laiko tarpus, į kuriuos 'telpa' mūsų paslaugos. Paslaugų laikas išgaunamas prieš vykdant šią procedurą
						*/
						WHILE(@CurrentRow < @MaxRow)
						BEGIN
							SET @NextRow = @CurrentRow + 1
							SET @minutes = 15
							IF(
									(
										SELECT[MinuteDiff]
										FROM @ModifiedTimes
										WHERE row = @CurrentRow
										) = 15
									)
								WHILE(1 = 1 AND @NextRow < @MaxRow)
								BEGIN
									SET @minutes = @minutes + 15

									IF(
											(
												SELECT[MinuteDiff]
												FROM @ModifiedTimes
												WHERE row = @NextRow
												) <> 15
											)
										BREAK
									SET @NextRow = @NextRow + 1
								END

							INSERT INTO @AvailableTimesWithTheirMinutes
							SELECT TheTime
								, @minutes
							FROM @ModifiedTimes
							WHERE row = @CurrentRow

							SET @CurrentRow = @CurrentRow + 1
						END
						/*
						--iškart po gavimo TIK laisvų laikų sugeneruojam lentelę, kurią Ajax gaus JSON tipu iš modelio ir JavaScript sukurs lentelę, tai padarius JavaScript pašalins 
						--null(tuščius) laukus. Struktūra, kuri yra grąžinama iš šios procedūros:
						--    Valanda1    |   Valanda2  | ...   <- valanda saugojama
						--  valanda1:00   | valanda2:00 | ...   <- minutės saugojamos (šis eilutė yra  0min)
						--  valanda1:15   | valanda2:15 | ...   <- minutės saugojamos (šis eilutė yra 15min)
						--  valanda1:30   | valanda2:30 | ...   <- minutės saugojamos (šis eilutė yra 30min)
						--  valanda1:45   | valanda2:45 | ...   <- minutės saugojamos (šis eilutė yra 45min)
						*/
						SELECT cast(ROW_NUMBER() OVER(ORDER BY CAST(GETDATE() AS TIMESTAMP)) as int) Id, [Hour00], [Hour01], [Hour02], [Hour03], [Hour04], [Hour05], [Hour06], [Hour07],
						  [Hour08], [Hour09], [Hour10], [Hour11], [Hour12], [Hour13], [Hour14], [Hour15], [Hour16], [Hour17], [Hour18], [Hour19], [Hour20], [Hour21], [Hour22], [Hour23]
								FROM(
							SELECT[MinuteInterval]
								,[TheTime]
								,[HourInverval]
							FROM(
								SELECT[TheTime]
									, substring(cast([TheTime] AS VARCHAR(20)), 4, 2)[MinuteInterval]
									, 'Hour' + cast([TheTime] AS VARCHAR(2))[HourInverval]
								FROM @AvailableTimesWithTheirMinutes
								WHERE[Minutes] >= @ServiceEstimatedTime
								) yt
							) src
						pivot(min([TheTime]) FOR[HourInverval] IN([Hour00], [Hour01], [Hour02], [Hour03], [Hour04], [Hour05], [Hour06], [Hour07], [Hour08], [Hour09], [Hour10], [Hour11],
						 [Hour12], [Hour13], [Hour14], [Hour15], [Hour16], [Hour17], [Hour18], [Hour19], [Hour20], [Hour21], [Hour22], [Hour23])) piv
					END";

			var sp_GetEmployeeNonWorkingDays = @"CREATE PROCEDURE [dbo].[GetEmployeeNonWorkingDays] @EmployeeId INT

				AS
				BEGIN
					SET NOCOUNT ON;
							SELECT[TheDate]
				  FROM[dbo].[DateTable]
				  inner join[dbo].[EmployeeSchedules] on[DateTable].TheDayOfWeek =[EmployeeSchedules].Day
				  where TheDate between cast(getdate() as date) and cast(dateadd(day,30,getdate()) as date)
				  and isnull(datediff(minute,[StartTime],[EndTime]),0)= 0
				  and[EmployeeSchedules].EmployeeId = @EmployeeId


				END
				GO";
			var sp_GetServiceEstimatedTimeSum = @"CREATE PROCEDURE [dbo].[GetServiceEstimatedTimeSum] @ServiceId1 INT = 0, @ServiceId2 INT = 0, @ServiceId3 INT = 0

					AS
					BEGIN
						SET NOCOUNT ON;
					SELECT sum([EstimatedTime])[EstimatedTime]
					  FROM [dbo].[Services]
					  where ID in (@ServiceId1,@ServiceId2,@ServiceId3)


					END
					GO";
								var sp_LastVerifyEmployeeAvailableTime = @"CREATE PROCEDURE [dbo].[LastVerifyEmployeeAvailableTime] @EmployeeId INT
						, @ServiceDate DATE
						,@ServiceEstimatedTime SMALLINT
						, @ProvidedTime time(7)
					AS
					BEGIN
						SET NOCOUNT ON;
								/*
								declare @EmployeeId INT =11
								,@ServiceDate DATE= '2020-04-10'
								,@ServiceEstimatedTime SMALLINT  = 150
								,@ProvidedTime time(7) = '10:30'
								*/

								DECLARE @AvailableTimes TABLE(
									row TINYINT
									, TheTime TIME(7)
									)

						/*
					SELECT TheDate, TheYear, TheMonth, TheDay, TheDayOfWeek, TheTime, StartTime, EndTime, BreakStartTime, BreakEndTime, [EmployeeSchedules].EmployeeId
							*/
						INSERT INTO @AvailableTimes(
							row
							, TheTime
							)
						SELECT ROW_NUMBER() OVER(
								ORDER BY TheTime
								) AS row
							, TheTime
						FROM[dbo].[DateTable]
						CROSS JOIN[dbo].[TimeTable]
								INNER JOIN[dbo].[EmployeeSchedules] ON[EmployeeSchedules].Day = [DateTable].TheDayOfWeek
						WHERE[DateTable].TheDate = @ServiceDate AND([TimeTable].TheTime BETWEEN StartTime AND EndTime) --laikai darbo laiku
							AND[TimeTable].TheTime NOT BETWEEN isnull(BreakStartTime, '00:00:00.0000000') AND CASE WHEN(BreakEndTime IS NULL) OR(BreakEndTime = '00:00:00.0000000') THEN '00:00:00.0000000' ELSE cast(dateadd(second, -1, BreakEndTime) AS TIME) END-- neiskaitomi laikai, kuriu metu yra pertrauka
							   -- sukuriam tai dienai visus galimus laikus, kuriu metu darbuotojas gali atlikti paslauga
					   AND[EmployeeSchedules].EmployeeId = @EmployeeId AND NOT EXISTS(
						  SELECT 1

						  FROM[dbo].[Reservations]

						  WHERE[Reservations].EmployeeId = @EmployeeId AND @ServiceDate = cast([Reservations].[Start] AS DATE) AND([TimeTable].TheTime BETWEEN cast([Reservations].[Start] AS TIME) AND cast(dateadd(second, -1, [Reservations].[End]) AS TIME)) AND[Reservations].StatusId <> 4
								) --tikrinam, kad musu diena tuo laiku butu laisvas laikas is neatsauktu laiku (id = 4 yra atsaukta)

						DECLARE @ModifiedTimes TABLE(
							row TINYINT
							, TheTime TIME(7)
							, NextAvailableTime TIME(7)
							, MinuteDiff INT
							)

						INSERT INTO @ModifiedTimes
						SELECT row
							,TheTime
							,lead(TheTime) OVER(
								ORDER BY row
								) NextAvailableTime
							,datediff(minute, TheTime, lead(TheTime) OVER(
									ORDER BY row
									)) MinuteDiff
						FROM @AvailableTimes

						DECLARE @CurrentRow INT = (
								SELECT min(row)
								FROM @ModifiedTimes
								)
						DECLARE @MaxRow INT = (
								SELECT max(row)
								FROM @ModifiedTimes
								)
						DECLARE @NextRow INT
						DECLARE @minutes INT
						DECLARE @AvailableTimesWithTheirMinutes TABLE(
							TheTime TIME(7)
							,[Minutes] SMALLINT
							)

						WHILE(@CurrentRow < @MaxRow)
						BEGIN
							SET @NextRow = @CurrentRow + 1
							SET @minutes = 15

							IF(
									(
										SELECT[MinuteDiff]
										FROM @ModifiedTimes
										WHERE row = @CurrentRow
										) = 15
									)
								WHILE(1 = 1 AND @NextRow < @MaxRow)
								BEGIN
									SET @minutes = @minutes + 15

									IF(
											(
												SELECT[MinuteDiff]
												FROM @ModifiedTimes
												WHERE row = @NextRow
												) <> 15
											)
										BREAK

									SET @NextRow = @NextRow + 1
								END

							INSERT INTO @AvailableTimesWithTheirMinutes
							SELECT TheTime
								, @minutes
							FROM @ModifiedTimes
							WHERE row = @CurrentRow

							SET @CurrentRow = @CurrentRow + 1
						END

						select 1 Id,cast(iif(count(*) = 1, 1, 0) as bit)[Boolean] from @AvailableTimesWithTheirMinutes
   						where TheTime = @ProvidedTime and Minutes>= @ServiceEstimatedTime

					END
					GO";
            migrationBuilder.Sql(sp_GetEmployeeAvailableTime);
			migrationBuilder.Sql(sp_GetEmployeeNonWorkingDays);
			migrationBuilder.Sql(sp_GetServiceEstimatedTimeSum);
			migrationBuilder.Sql(sp_LastVerifyEmployeeAvailableTime);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			/*
				DROP PROCEDURE [dbo].[GetEmployeeAvailableTime]
				DROP PROCEDURE [dbo].GetEmployeeNonWorkingDays
				DROP PROCEDURE [dbo].GetServiceEstimatedTimeSum
				DROP PROCEDURE [dbo].LastVerifyEmployeeAvailableTime
			 */

		}
	}
}
