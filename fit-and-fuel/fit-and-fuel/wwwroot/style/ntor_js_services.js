class PTServicesHandler extends elementorModules.frontend.handlers.Base {
  getDefaultSettings() {
    return {
      selectors: {
        swiper: '.swiper-container',
        prevEl: '.pt-swiper-button-prev',
        nextEl: '.pt-swiper-button-next',
        paginationEl: '.pt-swiper-pagination',
        scrollbarEl: '.swiper-scrollbar',
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
      $scrollbarEl: this.$element.find(selectors.scrollbarEl),
    }
  }

  initSwiper() {
    const elementSettings = this.getElementSettings();
    const elements = this.getDefaultElements();
    const elementorBreakpoints = elementorFrontend.config.responsive.activeBreakpoints;

    if ('carousel' != elementSettings.layout) {
      return;
    }

    const swiperOptions = {
      slidesPerView: elementSettings.cols,
      spaceBetween: 30,
      loop: 'yes' === elementSettings.loop,
      autoplay: 'yes' === elementSettings.autoplay,
      handleElementorBreakpoints: true,
    };

    swiperOptions.breakpoints = {};

    Object.keys(elementorBreakpoints).reverse().forEach((breakpointName) => {
      swiperOptions.breakpoints[elementorBreakpoints[breakpointName].value] = {
        slidesPerView: +elementSettings['cols_' + breakpointName]
      };
    });

    if ( 'yes' === elementSettings.arrows ) {
      swiperOptions.navigation = {
        nextEl: elements.$nextEl.get(0),
        prevEl: elements.$prevEl.get(0),
      };
    }

    if ( 'yes' === elementSettings.dots ) {
      swiperOptions.pagination = {
        el: elements.$paginationEl.get(0),
        type: 'bullets',
        clickable: true
      };
    }

    if ('yes' === elementSettings.scrollbar) {
      swiperOptions.scrollbar = {
        el: elements.$scrollbarEl.get(0),
        draggable: true
      };
    }

    const Swiper = elementorFrontend.utils.swiper;

    this.swiper = new Swiper(elements.$swiperContainer.get(0), swiperOptions);
  }

  onInit() {
    this.initSwiper();
  }
}

jQuery(window).on('elementor/frontend/init', () => {
  elementorFrontend.elementsHandler.attachHandler('pt-services', PTServicesHandler);
});
