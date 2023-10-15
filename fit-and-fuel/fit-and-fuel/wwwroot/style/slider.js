class PTSliderHandler extends elementorModules.frontend.handlers.Base {
  getDefaultSettings() {
    return {
      selectors: {
        swiper: '.pt-slider .swiper-container',
        prevEl: '.pt-swiper-button-prev',
        nextEl: '.pt-swiper-button-next',
        paginationEl: '.pt-swiper-fraction',
      }
    };
  }

  getDefaultElements() {
    const selectors = this.getSettings('selectors');

    return {
      $swiperContainer: this.$element.find(selectors.swiper),
      $prevEl: this.$element.find(selectors.prevEl),
      $nextEl: this.$element.find(selectors.nextEl),
      $paginationEl: this.$element.find(selectors.paginationEl),
    }
  }

  initSwiper() {
    const elementSettings = this.getElementSettings();
    const elements = this.getDefaultElements();

    const swiperOptions = {
      loop: 'yes' === elementSettings.loop,
      autoplay: 'yes' === elementSettings.autoplay,
    };

    if ( 'yes' === elementSettings.arrows ) {
      swiperOptions.navigation = {
        nextEl: elements.$nextEl.get(0),
        prevEl: elements.$prevEl.get(0),
      };
    }

    if ( 'yes' === elementSettings.dots ) {
      swiperOptions.pagination = {
        el: elements.$paginationEl.get(0),
        type: 'fraction',
        formatFractionCurrent: function(number) {
          return '0' + number;
        },
        formatFractionTotal: function(number) {
          return '0' + number;
        }
      };
    }

    if ( '' !== elementSettings.autoplay_delay && 'yes' === elementSettings.autoplay ) {
      swiperOptions.autoplay = {
        delay: elementSettings.autoplay_delay
      }
    }

    const slider = new Swiper(elements.$swiperContainer.get(0), swiperOptions);
  }

  onInit() {
    this.initSwiper();
  }
}

jQuery(window).on('elementor/frontend/init', () => {
  elementorFrontend.elementsHandler.attachHandler('pt-slider', PTSliderHandler);
});
