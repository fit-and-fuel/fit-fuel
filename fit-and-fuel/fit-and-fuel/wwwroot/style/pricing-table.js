class PTPricingTableHandler extends elementorModules.frontend.handlers.Base {
  getDefaultSettings() {
    return {
      selectors: {
        switcher: '.pt-pricing-table-switcher',
        swiper: '.swiper-container',
        prevEl: '.pt-swiper-button-prev',
        nextEl: '.pt-swiper-button-next',
        paginationEl: '.pt-swiper-pagination',
      }
    };
  }

  getDefaultElements() {
    const selectors = this.getSettings('selectors');

    return {
      $switcher: this.$element.find(selectors.switcher),
      $swiperContainer: this.$element.find(selectors.swiper),
      $prevEl: this.$element.find(selectors.prevEl),
      $nextEl: this.$element.find(selectors.nextEl),
      $paginationEl: this.$element.find(selectors.paginationEl),
    }
  }

  switcher() {
    const elements = this.getDefaultElements();
    
    const switcher = elements.$switcher.get(0);

    jQuery(switcher).on('click', '.pt-pricing-table-toggle', function () {
      jQuery(this).parents('.pt-pricing-tables').toggleClass('is-active');
    });
  }

  initSwiper() {
    const elementSettings = this.getElementSettings();
    const elements = this.getDefaultElements();
    const elementorBreakpoints = elementorFrontend.config.responsive.activeBreakpoints;

    const swiperOptions = {
      slidesPerView: elementSettings.cols,
      spaceBetween: 30,
      loop: 'yes' === elementSettings.loop,
      autoplay: 'yes' === elementSettings.autoplay,
      handleElementorBreakpoints: true,
      breakpoints: {
        0: {
          slidesPerView: 1
        },
        768: {
          slidesPerView: 2
        },
        1024: {
          slidesPerView: 2
        }
      }
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

    const Swiper = elementorFrontend.utils.swiper;

    this.swiper = new Swiper(elements.$swiperContainer.get(0), swiperOptions);
  }

  onInit() {
    this.initSwiper();
    this.switcher();
  }
}

jQuery(window).on('elementor/frontend/init', () => {
  elementorFrontend.elementsHandler.attachHandler('pt-pricing-table', PTPricingTableHandler);
});
