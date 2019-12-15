using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace DeviceTests
{
    // TEST NOTES:
    //   - a human needs to accept permissions on all systems
    //  If no calendars are set up none will be returned at this stage
    //  Same goes for events
    public class Calendar_Tests
    {
        [Fact]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Calendar_List()
        {
            return Utils.OnMainThread(async () =>
            {
                var calendarList = await Calendars.GetCalendarsAsync();
                Assert.NotNull(calendarList);
            });
        }

        [Fact]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Events_List()
        {
            return Utils.OnMainThread(async () =>
            {
                var eventList = await Calendars.GetEventsAsync();
                Assert.NotNull(eventList);
            });
        }

        [Theory]
        [InlineData("ThisIsAFakeId")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Events_By_Bad_Calendar_Text_Id(string calendarId)
        {
            return Utils.OnMainThread(async () =>
            {
#if __ANDROID__
                await Assert.ThrowsAsync<ArgumentException>(() => Calendars.GetEventsAsync(calendarId));
#else
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Calendars.GetEventsAsync(calendarId));
#endif
            });
        }

        [Theory]
        [InlineData("-1")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Events_By_Bad_Calendar_Id(string calendarId)
        {
            return Utils.OnMainThread(async () =>
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Calendars.GetEventsAsync(calendarId));
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Event_By_Blank_Id(string eventId)
        {
            return Utils.OnMainThread(async () =>
            {
                await Assert.ThrowsAsync<ArgumentException>(() => Calendars.GetEventByIdAsync(eventId));
            });
        }

        [Theory]
        [InlineData("-1")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Event_By_Bad_Id(string eventId)
        {
            return Utils.OnMainThread(async () =>
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Calendars.GetEventByIdAsync(eventId));
            });
        }

        [Theory]
        [InlineData("ThisIsAFakeId")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Get_Event_By_Bad_Text_Id(string eventId)
        {
            return Utils.OnMainThread(async () =>
            {
#if __ANDROID__
                await Assert.ThrowsAsync<ArgumentException>(() => Calendars.GetEventByIdAsync(eventId));
#else
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Calendars.GetEventByIdAsync(eventId));
#endif
            });
        }
    }
}
